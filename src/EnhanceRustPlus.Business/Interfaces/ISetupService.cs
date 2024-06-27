namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ISetupService
    {
        Task SetupDiscord(ulong guildId, string categoryName, string roleName);
        Task SetupCategory(ulong guildId, string categoryName, Guid serverId, string? roleName = null);
    }
}
