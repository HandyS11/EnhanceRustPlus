using EnhanceRustPlus.Business.Exceptions;
using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Parameters;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Services
{
    public class UserService(ILogger<UserService> logger, IUnitOfWork uow) : IUserService
    {
        private readonly IRepositoryManager<User> _userRepo = uow.GetRepository<User>();

        /// <summary>
        /// Creates a new user with the specified Discord ID and Steam ID.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="steamId">The Steam ID of the user.</param>
        /// <param name="name">The name of the user.</param>
        public async Task CreateUser(ulong discordId, ulong steamId, string name)
        {
            logger.LogEnteringMethod();

            try
            {
                var user = new User
                {
                    Id = discordId,
                    SteamId = steamId,
                    Name = name
                };

                await _userRepo.AddAsync(user);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error creating user");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
            }
        }

        /// <summary>
        /// Removes the user with the specified Discord ID.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user to remove.</param>
        public async Task RemoveUser(ulong discordId)
        {
            logger.LogEnteringMethod();

            try
            {
                var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
                if (user == null) throw new BusinessException("User not found");

                _userRepo.Delete(user);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error removing user");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
            }

            logger.LogExitingMethod();
        }

        /// <summary>
        /// Sets the credentials for the user with the specified Discord ID.
        /// </summary>
        /// <param name="credentials">The credentials to set.</param>
        /// <param name="discordId">The Discord ID of the user.</param>
        public async Task SetCredentials(CredentialsParameter credentials, ulong discordId)
        {
            logger.LogEnteringMethod();

            try
            {
                var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
                if (user == null) throw new BusinessException("User not found");

                var newCredentials = new Credential
                {
                    UserId = user.Id,
                    GcmAndroidId = credentials.GcmAndroidId,
                    GcmSecurityToken = credentials.GcmSecurityToken,
                    PrivateKey = credentials.PrivateKey,
                    PublicKey = credentials.PublicKey,
                    AuthSecret = credentials.AuthSecret
                };

                user.Credentials = newCredentials;

                _userRepo.Update(user);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error setting credentials");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
            }
        }

        /// <summary>
        /// Removes the credentials for the user with the specified Discord ID.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        public async Task RemoveCredentials(ulong discordId)
        {
            logger.LogEnteringMethod();

            try
            {
                var user = _userRepo.GetAsIQueryable().Include(u => u.Credentials).FirstOrDefault(x => x.Id == discordId);
                if (user == null) throw new BusinessException("User not found");

                user.Credentials = null;

                _userRepo.Update(user);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error removing user");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
            }

            logger.LogExitingMethod();
        }
    }
}
