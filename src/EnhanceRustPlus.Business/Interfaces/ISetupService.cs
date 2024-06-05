namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ISetupService
    {
        Task SetupDiscord(ulong guildId, string roleName, string categoryName);
    }
}
