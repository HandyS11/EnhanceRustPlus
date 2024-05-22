using Discord;
using Discord.WebSocket;
using EnhanceRustPlus.Extensions;
using EnhanceRustPlus.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SetupService(DiscordSocketClient client, ILogger<SetupService> logger) : ISetupService
    {
        public async Task<ulong> CreateRoleAsync(ulong guildId)
        {
            logger.LogEnteringMethod();

            var guild = client.GetGuild(guildId);

            var role = await guild.CreateRoleAsync("Rust+");

            logger.LogExitingMethod();

            return role.Id;
        }

        public async Task<ulong> CreateCategoryAsync(ulong guildId, ulong roleId)
        {
            logger.LogEnteringMethod();

            var guild = client.GetGuild(guildId);

            var category = await guild.CreateCategoryChannelAsync("Rust+", properties =>
            {
                properties.Position = 0;
            });

            await category.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            await category.AddPermissionOverwriteAsync(guild.GetRole(roleId), new OverwritePermissions(viewChannel: PermValue.Allow));

            logger.LogExitingMethod();

            return category.Id;
        }

        public async Task<Dictionary<ulong, string>> CreateChannelsAsync(ulong guildId, ulong categoryId)
        {
            logger.LogEnteringMethod();

            var guild = client.GetGuild(guildId);

            // TODO: Set in config file or so
            var channelNames = new List<string>
            {
                "information",
                "servers",
                "settings",
                "commands",
                "events",
                "team-chat",
                "switches",
                "alarms",
                "activity",
            };

            var channels = new Dictionary<ulong, string>();

            foreach (var name in channelNames)
            {
                var channel = await guild.CreateTextChannelAsync(name, properties =>
                {
                    properties.CategoryId = categoryId;
                });
                channels.Add(channel.Id, channel.Name);
            }

            logger.LogExitingMethod();

            return channels;
        }
    }
}
