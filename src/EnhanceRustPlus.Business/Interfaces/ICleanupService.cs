namespace EnhanceRustPlus.Business.Interfaces
{
    public interface ICleanupService
    {
        Task CleanupAsync(ulong guildId);
    }
}
