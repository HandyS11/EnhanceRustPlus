using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Serilog.Events;
using Serilog;

namespace EnhanceRustPlus.Utils
{
    /// <summary>
    /// Handles various events in the Discord client.
    /// </summary>
    public class EventHandlers(IServiceProvider serviceProvider, DiscordSocketClient client, InteractionService interactionService)
    {
        /// <summary>
        /// Logs a message asynchronously.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Performs actions when the client is ready.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Ready()
        {
            await client.SetGameAsync("Rust");
            await interactionService.RegisterCommandsGloballyAsync();
        }

        /// <summary>
        /// Handles the creation of interactions.
        /// </summary>
        /// <param name="interaction">The socket interaction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task InteractionCreated(SocketInteraction interaction)
        {
            var ctx = new SocketInteractionContext(client, interaction);
            return interactionService.ExecuteCommandAsync(ctx, serviceProvider);
        }
    }
}
