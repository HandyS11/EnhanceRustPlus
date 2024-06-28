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
            var aesIv = AesKeyGenerator.GenerateIV();

            var embed = new EmbedBuilder
            {
                Title = "AES Keys",
                Fields =
                [
                    new EmbedFieldBuilder { Name = "Key", Value = Convert.ToBase64String(aesKeys) },
                    new EmbedFieldBuilder { Name = "IV", Value = Convert.ToBase64String(aesIv) }
                ],
                Color = Color.Green
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
