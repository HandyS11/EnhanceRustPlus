using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Business.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(ulong discordId, ulong steamId, string name);
        Task RemoveUser(ulong discordId);
        Task SetCredentials(CredentialsParameter credentials, ulong discordId);
        Task RemoveCredentials(ulong discordId);
    }
}
