using Discord;
using Discord.Interactions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Parameters;

namespace EnhanceRustPlus.Commands
{
    [Group("user", "All utilities about users")]
    [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
    public class UserGroupCommand(IUserService userService) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("create", "Add a user")]
        public async Task CreateUser(ulong steamId, ulong discordId = 0, string? name = null, string? avatar = null)
        {
            if (discordId == 0) discordId = Context.User.Id;
            name ??= Context.User.Username;
            avatar ??= Context.User.GetAvatarUrl();

            try
            {
                await userService.CreateUser(discordId, steamId, name, avatar);
            }
            catch (Exception ex)
            {
                await RespondAsync($"Error: {ex.Message}", ephemeral: true);
            }

            await RespondAsync("User added", ephemeral: true);
        }

        [SlashCommand("update", "Update a user")]
        public async Task UpdateUser(ulong discordId, ulong steamId, string? name = null, string? avatar = null)
        {
            if (discordId == 0) discordId = Context.User.Id;

            try
            {
                await userService.UpdateUser(discordId, steamId, name, avatar);
            }
            catch (Exception ex)
            {
                await RespondAsync($"Error: {ex.Message}", ephemeral: true);
            }

            await RespondAsync("User updated", ephemeral: true);
        }

        [SlashCommand("register", "Register a user to a guild")]
        public async Task RegisterUser(ulong discordId = 0, ulong guildId = 0)
        {
            if (discordId == 0) discordId = Context.User.Id;
            if (guildId == 0) guildId = Context.Guild.Id;

            try
            {
                await userService.RegisterUser(discordId, guildId);
            }
            catch (Exception ex)
            {
                await RespondAsync($"Error: {ex.Message}", ephemeral: true);
            }

            await RespondAsync("User registered", ephemeral: true);
        }

        [SlashCommand("unregister", "Unregister a user from a guild")]
        public async Task UnregisterUser(ulong discordId = 0, ulong guildId = 0)
        {
            if (discordId == 0) discordId = Context.User.Id;
            if (guildId == 0) guildId = Context.Guild.Id;

            try
            {
                await userService.UnregisterUser(discordId, guildId);
            }
            catch (Exception ex)
            {
                await RespondAsync($"Error: {ex.Message}", ephemeral: true);
            }

            await RespondAsync("User unregistered", ephemeral: true);
        }

        [SlashCommand("delete", "Remove a user")]
        public async Task DeleteUser(ulong discordId = 0)
        {
            if (discordId == 0) discordId = Context.User.Id;

            try
            {
                await userService.DeleteUser(discordId);
            }
            catch (Exception ex)
            {
                await RespondAsync($"Error: {ex.Message}", ephemeral: true);
            }

            await RespondAsync("User removed", ephemeral: true);
        }

        [Group("credentials", "All utilities about credentials")]
        public class CredentialsGroupCommand(ICredentialService credentialService) : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("set", "Set the credentials for a user")]
            public async Task SetCredentials([ComplexParameter] CredentialsParameter credentials, ulong discordId = 0)
            {
                if (discordId == 0) discordId = Context.User.Id;

                try
                {
                    await credentialService.SetCredentials(discordId, credentials);
                }
                catch (Exception ex)
                {
                    await RespondAsync($"Error: {ex.Message}", ephemeral: true);
                }

                await RespondAsync("Credentials set", ephemeral: true);
            }

            [SlashCommand("remove", "Remove credentials for a user")]
            public async Task RemoveCredentials(ulong discordId = 0)
            {
                if (discordId == 0) discordId = Context.User.Id;

                try
                {
                    await credentialService.RemoveCredentials(discordId);
                }
                catch (Exception ex)
                {
                    await RespondAsync($"Error: {ex.Message}", ephemeral: true);
                }

                await RespondAsync("Credentials removed", ephemeral: true);
            }
        }
    }
}
