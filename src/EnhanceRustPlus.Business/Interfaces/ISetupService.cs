namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ISetupService
    {
        Task<bool> SetupDiscord(ulong guildId, string roleName, string categoryName);
    }
}
