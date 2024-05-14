using Discord.WebSocket;

namespace EnhanceRustPlus.Utils
{
    public static class ChannelUtils
    {
        public static async Task<ulong> CreateTextChannelAsync(SocketGuild guild, string name, ulong? categoryId = null)
        {
            var channel = await guild.CreateTextChannelAsync(name, properties =>
            {
                properties.CategoryId = categoryId;
            });
            return channel.Id;
        }

        public static async void DeleteTextChannelAsync(SocketGuild guild, ulong id)
        {
            await guild.GetTextChannel(id).DeleteAsync();
        }
    }
}
