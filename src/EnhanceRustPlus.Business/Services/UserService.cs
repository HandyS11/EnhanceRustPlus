using EnhanceRustPlus.Business.Exceptions;
using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Services
{
    /// <summary>
    /// Represents a service for managing users.
    /// </summary>
    public class UserService(ILogger<UserService> logger, IUnitOfWork uow) : IUserService
    {
        private readonly IRepositoryManager<Guild> _guildRepo = uow.GetRepository<Guild>();
        private readonly IRepositoryManager<GuildUser> _guildUserRepo = uow.GetRepository<GuildUser>();
        private readonly IRepositoryManager<User> _userRepo = uow.GetRepository<User>();

        /// <summary>
        /// Creates a new user with the specified Discord ID and Steam ID.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="steamId">The Steam ID of the user.</param>
        /// <param name="name">The name of the user.</param>
        /// <returns>Returns true if the user is created successfully, otherwise false.</returns>
        public async Task<bool> CreateUser(ulong discordId, ulong steamId, string name)
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
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
                return false;
            }

            logger.LogExitingMethod();

            return true;
        }

        /// <summary>
        /// Registers a user to a guild.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="guildId">The ID of the guild.</param>
        /// <returns>Returns true if the user is registered successfully, otherwise false.</returns>
        public async Task<bool> RegisterUser(ulong discordId, ulong guildId)
        {
            logger.LogEnteringMethod();

            try
            {
                var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
                if (user == null) throw new BusinessException("User not found");

                var guild = _guildRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == guildId);
                if (guild == null) throw new BusinessException("Guild not found");

                var userGuild = _guildUserRepo.GetAsIQueryable().FirstOrDefault(x => x.GuildId == guildId && x.UserId == discordId);
                if (userGuild != null) throw new BusinessException("User already registered to the guild");

                var guildUser = new GuildUser
                {
                    GuildId = guildId,
                    UserId = discordId
                };
                await _guildUserRepo.AddAsync(guildUser);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error registering user");
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
                return false;
            }

            logger.LogExitingMethod();

            return true;
        }

        /// <summary>
        /// Removes the user with the specified Discord ID.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user to remove.</param>
        /// <returns>Returns true if the user is removed successfully, otherwise false.</returns>
        public async Task<bool> RemoveUser(ulong discordId)
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
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unknown Error");
                return false;
            }

            logger.LogExitingMethod();

            return true;
        }
    }
}
