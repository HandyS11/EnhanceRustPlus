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
        private readonly IRepositoryManager<Credential> _guildRepo = uow.GetRepository<Credential>();

        public Task AddCredentails(CredentialsParameter credentials, ulong id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCredentails(ulong id)
        {
            throw new NotImplementedException();
        }
    }
}
