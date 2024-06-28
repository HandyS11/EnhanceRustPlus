using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ICredentialService
    {
        Task SetCredentials(ulong discordId, CredentialsParameter credentials);
        Task RemoveCredentials(ulong discordId);
    }
}
