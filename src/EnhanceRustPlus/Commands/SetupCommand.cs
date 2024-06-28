using Discord;
using Discord.Interactions;

using EnhanceRustPlus.Business.Interfaces;

namespace EnhanceRustPlus.Commands
{
    public class SetupCommand(ISetupService service) : InteractionModuleBase
    {
        [SlashCommand("setup", "Setup the category, role, channels and messages to operate the application")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        public async Task Setup()
        {
            _ = service.SetupDiscord(Context.Guild.Id, "Rust+", "Rust+");

            var embed = new EmbedBuilder
            {
                Description = "Setup started..",
                Color = Color.Green,
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
