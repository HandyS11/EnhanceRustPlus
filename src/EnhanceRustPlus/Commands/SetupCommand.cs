using Discord;
using Discord.Interactions;
using EnhanceRustPlus.Business.Interfaces;

namespace EnhanceRustPlus.Commands
{
    public class SetupCommand(ISetupService service) : InteractionModuleBase
    {
        [SlashCommand("setup", "Setup the category, role, channels and message to operate the application")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireRole("Rust+", Group = "Permission")]
        public async Task Setup()
        {
            var setup = await service.SetupDiscord(Context.Guild.Id, "Rust+", "Rust+");
            // ...

            var embed = new EmbedBuilder
            {
                Description = "Setup completed"
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
