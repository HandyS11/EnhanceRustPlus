namespace EnhanceRustPlus.Business.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUser(ulong discordId, ulong steamId, string name);
        Task<bool> RegisterUser(ulong discordId, ulong guildId);
        Task<bool> RemoveUser(ulong discordId);
    }
}
