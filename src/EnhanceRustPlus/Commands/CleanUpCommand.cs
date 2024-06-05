using Discord;
using Discord.Interactions;

using EnhanceRustPlus.Business.Interfaces;

namespace EnhanceRustPlus.Commands
{
    public class CleanUpCommand(ICleanupService service) : InteractionModuleBase
    {
        [SlashCommand("clean-up", "Clean-up the category, role and channels")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        public async Task CleanUp()
        {
            _ = service.CleanupAsync(Context.Guild.Id);

            var embed = new EmbedBuilder
            {
                Description = "Clean-up started..",
                Color = Color.Green
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
