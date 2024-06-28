using EnhanceRustPlus.Business.Exceptions;
using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Helpers;
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
        /// Sets the credentials for a user.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        /// <param name="credentials">The credentials to set.</param>
        public async Task SetCredentials(ulong discordId, CredentialsParameter credentials)
        {
            logger.LogEnteringMethod();

            var user = _userRepo.GetAsIQueryable().FirstOrDefault(x => x.Id == discordId);
            if (user == null) throw new BusinessException(Constants.UserNotFoundInDatabase);

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

            try
            {
                _userRepo.Update(user);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }

        /// <summary>
        /// Removes the credentials for a user.
        /// </summary>
        /// <param name="discordId">The Discord ID of the user.</param>
        public async Task RemoveCredentials(ulong discordId)
        {
            logger.LogEnteringMethod();

            var user = _userRepo.GetAsIQueryable().Include(u => u.Credentials).FirstOrDefault(x => x.Id == discordId);
            if (user == null) throw new BusinessException(Constants.UserNotFoundInDatabase);

            user.Credentials = null;

            try
            {
                _userRepo.Update(user);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException(Constants.CannotSaveTransactionToDatabase, e);
            }

            logger.LogExitingMethod();
        }
    }
}
