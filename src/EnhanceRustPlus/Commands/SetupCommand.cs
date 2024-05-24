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
            var roleId = await service.CreateRoleAsync(Context.Guild.Id);
            var categoryId = await service.CreateCategoryAsync(Context.Guild.Id, roleId);
            var channels = await service.CreateChannelsAsync(Context.Guild.Id, categoryId);

            // TODO: Manage database to store the data

            var embed = new EmbedBuilder
            {
                Description = "Setup completed"
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
