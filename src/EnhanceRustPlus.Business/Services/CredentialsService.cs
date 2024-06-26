using EnhanceRustPlus.Business.Extensions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Parameters;
using EnhanceRustPlus.EfCore.Entities;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Services
{
    public class CredentialsService(ILogger<CredentialsService> logger, IUnitOfWork uow) : ICredentialsService
    {
        private readonly IRepositoryManager<Credentials> _guildRepo = uow.GetRepository<Credentials>();

        public async Task AddCredentails(CredentialsParameter credentials, ulong id)
        {
            logger.LogEnteringMethod();

            try
            {
                var credential = _guildRepo.GetAsIQueryable()
                    .FirstOrDefault(x => x.Id == id);

                if (credential != null) logger.LogAndThrowBusinessException("Credentials already exist in database");

                await _guildRepo.AddAsync(new Credentials
                {
                    Id = id,
                    GcmAndroidId = credentials.GcmAndroidId,
                    GcmSecurityToken = credentials.GcmSecurityToken,
                    PrivateKey = credentials.PrivateKey,
                    PublicKey = credentials.PublicKey,
                    AuthSecret = credentials.AuthSecret
                });
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException("Error adding credentials to database", e);
            }

            logger.LogExitingMethod();
        }

        public async Task RemoveCredentails(ulong id)
        {
            logger.LogEnteringMethod();

            try
            {
                var credentials = _guildRepo.GetAsIQueryable()
                    .FirstOrDefault(x => x.Id == id);

                if (credentials == null) logger.LogAndThrowBusinessException("Credentials not found in database");

                await _guildRepo.DeleteAsync(credentials!.Id);
                await uow.SaveAsync();
            }
            catch (Exception e)
            {
                logger.LogAndThrowBusinessException("Error removing credentials from database", e);
            }

            logger.LogExitingMethod();
        }
    }
}
