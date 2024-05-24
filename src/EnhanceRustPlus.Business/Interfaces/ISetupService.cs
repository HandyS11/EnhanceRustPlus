namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ISetupService
    {
        Task<ulong> CreateRoleAsync(ulong guildId);
        Task<ulong> CreateCategoryAsync(ulong guildId, ulong roleId);
        Task<Dictionary<ulong, string>> CreateChannelsAsync(ulong guildId, ulong categoryId);
    }
}
