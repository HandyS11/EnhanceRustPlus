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

            var embed = new EmbedBuilder
            {
                Description = setup ? "Setup completed" : "Setup failed",
                Color = setup ? Color.Green : Color.Red
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
