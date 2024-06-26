namespace EnhanceRustPlus.Configuration
{
    public class BotSettings
    {
        public string DiscordBotToken { get; set; } = null!;
        public EncryptionSettings EncryptionSettings { get; set; } = null!;
    }

    public class EncryptionSettings
    {
        public string Key { get; set; } = null!;
        public string IV { get; set; } = null!;
    }
}