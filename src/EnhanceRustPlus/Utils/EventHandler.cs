using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Serilog.Events;
using Serilog;

namespace EnhanceRustPlus.Utils
{
    public class EventHandlers(IServiceProvider serviceProvider, DiscordSocketClient client, InteractionService interactionService)
    {
        public static async Task LogAsync(LogMessage message)
        {
            var severity = message.Severity switch
            {
                LogSeverity.Critical => LogEventLevel.Fatal,
                LogSeverity.Error => LogEventLevel.Error,
                LogSeverity.Warning => LogEventLevel.Warning,
                LogSeverity.Info => LogEventLevel.Information,
                LogSeverity.Verbose => LogEventLevel.Verbose,
                LogSeverity.Debug => LogEventLevel.Debug,
                _ => LogEventLevel.Information
            };
            Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);

            await Task.CompletedTask;
        }

        public async Task Ready()
        {
            await client.SetGameAsync("Rust");
            await interactionService.RegisterCommandsGloballyAsync();
        }

        public Task InteractionCreated(SocketInteraction interaction)
        {
            var ctx = new SocketInteractionContext(client, interaction);
            return interactionService.ExecuteCommandAsync(ctx, serviceProvider);
        }
    }
}
