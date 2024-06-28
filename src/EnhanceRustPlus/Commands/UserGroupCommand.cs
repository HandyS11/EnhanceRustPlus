using Discord;
using Discord.Interactions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Commands
{
    [Group("user", "All utilities around users")]
    [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
    public class UserGroupCommand(IUserService userService) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("add", "Add a user")]
        public async Task AddUser(ulong steamId, ulong discordId = 0, string? name = null)
        {
            if (discordId == 0) discordId = Context.User.Id;
            name ??= Context.User.Username;

            var result = await userService.CreateUser(discordId, steamId, name);

            await RespondAsync(result ? "User added" : "Error: Failed to add user", ephemeral: true);
        }

        [SlashCommand("register", "Register a user to a guild")]
        public async Task RegisterUser(ulong discordId = 0, ulong guildId = 0)
        {
            if (discordId == 0) discordId = Context.User.Id;
            if (guildId == 0) guildId = Context.Guild.Id;

            var result = await userService.RegisterUser(discordId, guildId);

            await RespondAsync(result ? "User registered" : "Error: Failed to register user", ephemeral: true);
        }

        [SlashCommand("remove", "Remove a user")]
        public async Task RemoveUser(ulong discordId = 0)
        {
            if (discordId == 0)
                discordId = Context.User.Id;

            var result = await userService.RemoveUser(discordId);

            await RespondAsync(result ? "User removed" : "Error: Failed to remove user", ephemeral: true);
        }

        [Group("credentials", "All utilities credentials")]
        public class CredentialsGroupCommand(ICredentialService credentialService) : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("set", "Set the credentials for a user")]
            public async Task SetCredentials(CredentialsParameter credentials, ulong discordId = 0)
            {
                if (discordId == 0)
                    discordId = Context.User.Id;

                var result = await credentialService.SetCredentials(discordId, credentials);

                await RespondAsync(result ? "Credentials set" : "Error: Failed to set credentials", ephemeral: true);
            }

            [SlashCommand("remove", "Remove credentials for a user")]
            public async Task RemoveCredentials(ulong discordId = 0)
            {
                if (discordId == 0)
                    discordId = Context.User.Id;

                var result = await credentialService.RemoveCredentials(discordId);

                await RespondAsync(result? "Credentials removed" : "Error: Failed to remove credentials", ephemeral: true);
            }
        }
    }
}
