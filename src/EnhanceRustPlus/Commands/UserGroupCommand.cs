using Discord;
using Discord.Interactions;
using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Parameters;
using static EnhanceRustPlus.Utils.EmbedBuilderHelper;

namespace EnhanceRustPlus.Commands
{
    [Group("user", "All utilities about the users")]
    [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
    public class UserGroupCommand(IUserService userService) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("create", "Create a user")]
        public async Task CreateUser(
            [Summary("steam-id", "Steam ID of the user")] ulong steamId,
            [Summary("discord-id", "Discord ID of the user (can be null if yours)")] ulong discordId = 0,
            [Summary("display-name", "Name of the user (can be null if yours)")] string? name = null,
            [Summary("avatar-url", "Avatar URL of the user (can be null if yours)")] string? avatar = null)
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
                await RespondAsync(embed: ErrorEmbed(ex).Build(), ephemeral: true);
            }

            await RespondAsync("User added", ephemeral: true);
        }

        [SlashCommand("update", "Update a user")]
        public async Task UpdateUser(
            [Summary("steam-id", "Steam ID of the user")] ulong steamId,
            [Summary("discord-id", "Discord ID of the user (can be null if yours)")] ulong discordId = 0,
            [Summary("display-name", "Name of the user (can be null)")] string? name = null,
            [Summary("avatar-url", "Avatar URL of the user (can be null)")] string? avatar = null)
        {
            if (discordId == 0) discordId = Context.User.Id;

            try
            {
                await userService.UpdateUser(discordId, steamId, name, avatar);
            }
            catch (Exception ex)
            {
                await RespondAsync(embed: ErrorEmbed(ex).Build(), ephemeral: true);
            }

            await RespondAsync("User updated", ephemeral: true);
        }

        [SlashCommand("register", "Register a user to a guild")]
        public async Task RegisterUser(
            [Summary("discord-id", "Discord ID of the user (can be null if yours)")] ulong discordId = 0,
            [Summary("guild-id", "Guild ID of the user (can be null if yours)")] ulong guildId = 0)
        {
            if (discordId == 0) discordId = Context.User.Id;
            if (guildId == 0) guildId = Context.Guild.Id;

            try
            {
                await userService.RegisterUser(discordId, guildId);
            }
            catch (Exception ex)
            {
                await RespondAsync(embed: ErrorEmbed(ex).Build(), ephemeral: true);
            }

            await RespondAsync("User registered", ephemeral: true);
        }

        [SlashCommand("unregister", "Unregister a user from a guild")]
        public async Task UnregisterUser(
            [Summary("discord-id", "Discord ID of the user (can be null if yours)")] ulong discordId = 0,
            [Summary("guild-id", "Guild ID of the user (can be null if yours)")] ulong guildId = 0)
        {
            if (discordId == 0) discordId = Context.User.Id;
            if (guildId == 0) guildId = Context.Guild.Id;

            try
            {
                await userService.UnregisterUser(discordId, guildId);
            }
            catch (Exception ex)
            {
                await RespondAsync(embed: ErrorEmbed(ex).Build(), ephemeral: true);
            }

            await RespondAsync("User unregistered", ephemeral: true);
        }

        [SlashCommand("delete", "Delete a user")]
        public async Task DeleteUser(
            [Summary("discord-id", "Discord ID of the user (can be null if yours)")] ulong discordId = 0)
        {
            if (discordId == 0) discordId = Context.User.Id;

            try
            {
                await userService.DeleteUser(discordId);
            }
            catch (Exception ex)
            {
                await RespondAsync(embed: ErrorEmbed(ex).Build(), ephemeral: true);
            }

            await RespondAsync("User removed", ephemeral: true);
        }

        [Group("credentials", "All utilities about credentials")]
        public class CredentialsGroupCommand(ICredentialService credentialService) : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("set", "Set the credentials for a user")]
            public async Task SetCredentials(
                [ComplexParameter] CredentialsParameter credentials,
                [Summary("discord-id", "Discord ID of the user (can be null if yours)")] ulong discordId = 0)
            {
                if (discordId == 0) discordId = Context.User.Id;

                try
                {
                    await credentialService.SetCredentials(discordId, credentials);
                }
                catch (Exception ex)
                {
                    await RespondAsync(embed: ErrorEmbed(ex).Build(), ephemeral: true);
                }

                await RespondAsync("Credentials set", ephemeral: true);
            }

            [SlashCommand("remove", "Remove credentials for a user")]
            public async Task RemoveCredentials(
                [Summary("discord-id", "Discord ID of the user (can be null if yours)")] ulong discordId = 0)
            {
                if (discordId == 0) discordId = Context.User.Id;

                try
                {
                    await credentialService.RemoveCredentials(discordId);
                }
                catch (Exception ex)
                {
                    await RespondAsync(embed: ErrorEmbed(ex).Build(), ephemeral: true);
                }

                await RespondAsync("Credentials removed", ephemeral: true);
            }
        }
    }
}
