using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ICredentialService
    {
        Task<bool> SetCredentials(ulong discordId, CredentialsParameter credentials);
        Task<bool> RemoveCredentials(ulong discordId);
    }
}
