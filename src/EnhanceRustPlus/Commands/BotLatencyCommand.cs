using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace EnhanceRustPlus.Commands
{
    public class BotLatencyCommand(DiscordSocketClient client) : InteractionModuleBase
    {
        [SlashCommand("ping", "Gets the current bot latency")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireRole("Rust+", Group = "Permission")]
        public async Task Ping()
        {
            var embed = new EmbedBuilder
            {
                Description = $"Pong. Took **{client.Latency}ms** to respond",
                Footer = new EmbedFooterBuilder
                {
                    Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                }
            };
            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}
