using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ICredentialsService
    {
        Task AddCredentails(CredentialsParameter credentials, ulong id);
        Task RemoveCredentails(ulong id);
    }
}
