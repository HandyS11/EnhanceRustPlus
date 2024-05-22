namespace EnhanceRustPlus.Interfaces
{
    public interface ICleanupService
    {
        Task CleanupAsync(ulong guildId);
    }
}
