using Discord;
using Discord.Interactions;
using EnhanceRustPlus.Business.Helpers;

namespace EnhanceRustPlus.Commands
{
    public class GenerateAesKeysCommand : InteractionModuleBase
    {
        [SlashCommand("generate-aes-key", "Generate credentials for securing the database")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        public async Task GenerateAesKey()
        {
            var aesKeys = AesKeyGenerator.GenerateKey();
            var aesIV = AesKeyGenerator.GenerateIV();

            var embed = new EmbedBuilder
            {
                Title = "Generated AES Keys",
                Description = $"Key: {Convert.ToBase64String(aesKeys)}\nIV: {Convert.ToBase64String(aesIV)}",
                Color = Color.Green
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
