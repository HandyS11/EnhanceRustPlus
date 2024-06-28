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
    /// <summary>
    /// Service class for managing user credentials.
    /// </summary>
    public class CredentialService(ILogger<CredentialService> logger, IUnitOfWork uow, IEncryptionService encryptionService) : ICredentialService
    {
        private readonly IRepositoryManager<User> _userRepo = uow.GetRepository<User>();

        /// <summary>
        /// Sets the credentials for the user with the specified Discord ID.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="credentials">The credentials to set.</param>
        /// <returns>Returns true if the credentials are set successfully, otherwise false.</returns>
        public async Task<bool> SetCredentials(ulong discordId, CredentialsParameter credentials)
        {
            logger.LogEnteringMethod();

            try
            {
                var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
                if (user == null) throw new BusinessException("User not found");

                var newCredentials = new Credential
                {
                    UserId = user.Id,
                    GcmAndroidId = encryptionService.EncryptString(credentials.GcmAndroidId.ToString()),
                    GcmSecurityToken = encryptionService.EncryptString(credentials.GcmSecurityToken.ToString()),
                    PrivateKey = encryptionService.EncryptString(credentials.PrivateKey),
                    PublicKey = encryptionService.EncryptString(credentials.PublicKey),
                    AuthSecret = encryptionService.EncryptString(credentials.AuthSecret)
                };

                user.Credentials = newCredentials;

                _userRepo.Update(user);
                await uow.SaveAsync();
            }
            catch (BusinessException e)
            {
                logger.LogError(e, "Error setting credentials");
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
        /// Removes the credentials for the user with the specified Discord ID.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <returns>Returns true if the credentials are removed successfully, otherwise false.</returns>
        public async Task<bool> RemoveCredentials(ulong discordId)
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
