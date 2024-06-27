using Discord;
using Discord.Interactions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Commands
{
    [Group("user", "All utilities arround users")]
    [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
    public class UserGroupCommand(IUserService userService) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("add", "Add a users")]
        public async Task AddUser(ulong steamId, ulong discordId = 0, string? name = null)
        {
            if (discordId == 0) discordId = Context.User.Id;
            if (name == null) name = Context.User.Username;

            await userService.CreateUser(discordId, steamId, name);
            await RespondAsync("User added", ephemeral: true);
        }

        [SlashCommand("remove", "Remove a user")]
        public async Task RemoveUser(ulong discordId = 0)
        {
            if (discordId == 0)
                discordId = Context.User.Id;

            await userService.RemoveUser(discordId);
            await RespondAsync("User removed", ephemeral: true);
        }

        [Group("credentials", "All utilities credentials")]
        public class CredentialsGroupCommand(IUserService userService) : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("set", "Set the credentials for a user")]
            public async Task SetCredentials(CredentialsParameter credentials, ulong discordId = 0)
            {
                if (discordId == 0)
                    discordId = Context.User.Id;

                await userService.SetCredentials(credentials, discordId);
                await RespondAsync("Credentials set", ephemeral: true);
            }

            [SlashCommand("remove", "Remove credentials for a user")]
            public async Task RemoveCredentials(ulong discordId = 0)
            {
                if (discordId == 0)
                    discordId = Context.User.Id;

                await userService.RemoveCredentials(discordId);
                await RespondAsync("Credentials removed", ephemeral: true);
            }
        }
    }
}
