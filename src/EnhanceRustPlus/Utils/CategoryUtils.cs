using Discord.WebSocket;

namespace EnhanceRustPlus.Utils
{
    public class CategoryUtils
    {
        public static async Task<ulong> CreateCategoryAsync(SocketGuild guild, string name)
        {
            var category = await guild.CreateCategoryChannelAsync(name, properties =>
            {
                properties.Position = 0;
            });
            return category.Id;
        }
    }
}
