using Discord.WebSocket;
using EnhanceRustPlus.Utils;

namespace EnhanceRustPlus.Setup
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SetupApp(DiscordSocketClient client)
    {
        public async Task<List<ulong>> SetupCategory(ulong guildId)
        {
            var guild = client.GetGuild(guildId);

            var categoryId = await CategoryUtils.CreateCategoryAsync(guild, "Rust+");
            var channelIds = new List<ulong>
            {
                await ChannelUtils.CreateTextChannelAsync(guild, "information", categoryId),
                await ChannelUtils.CreateTextChannelAsync(guild, "servers", categoryId),
                await ChannelUtils.CreateTextChannelAsync(guild, "settings", categoryId),
                await ChannelUtils.CreateTextChannelAsync(guild, "commands", categoryId),
                await ChannelUtils.CreateTextChannelAsync(guild, "events", categoryId),
                await ChannelUtils.CreateTextChannelAsync(guild, "team-chat", categoryId),
                await ChannelUtils.CreateTextChannelAsync(guild, "alarms", categoryId),
                await ChannelUtils.CreateTextChannelAsync(guild, "activity", categoryId),
            };


            Thread.Sleep(5000);

            for (var i = channelIds.Count - 1; i >= 0; i--)
            {
                ChannelUtils.DeleteTextChannelAsync(guild, channelIds[i]);
                Thread.Sleep(500);
            }
            await guild.GetCategoryChannel(categoryId).DeleteAsync();

            return channelIds;
        }
    }
}
