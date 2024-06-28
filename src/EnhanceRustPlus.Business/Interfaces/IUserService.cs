namespace EnhanceRustPlus.Business.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUser(ulong discordId, ulong steamId, string name);
        Task<bool> UpdateUser(ulong discordId, ulong steamId = 0, string? name = null);
        Task<bool> RegisterUser(ulong discordId, ulong guildId);
        Task<bool> UnregisterUser(ulong discordId, ulong guildId);
        Task<bool> DeleteUser(ulong discordId);
    }
}
