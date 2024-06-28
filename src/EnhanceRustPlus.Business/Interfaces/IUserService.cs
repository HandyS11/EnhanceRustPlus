namespace EnhanceRustPlus.Business.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(ulong discordId, ulong steamId, string name, string avatar);
        Task UpdateUser(ulong discordId, ulong steamId = 0, string? name = null, string? avatar = null);
        Task RegisterUser(ulong discordId, ulong guildId);
        Task UnregisterUser(ulong discordId, ulong guildId);
        Task DeleteUser(ulong discordId);
    }
}
