using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;

namespace EnhanceRustPlus.Utils
{
    public class EventHandlers(IServiceProvider serviceProvider, DiscordSocketClient client, InteractionService interactionService)
    {
        public static Task LogMessage(LogMessage message)
        {
            if (message.Exception is CommandException exception)
            {
                Console.WriteLine($"[Command/{message.Severity}] {exception.Command.Aliases[0]} "
                                  + $"failed to execute in {exception.Context.Channel}");
                Console.WriteLine(exception);
                return Task.CompletedTask;
            }

            Console.WriteLine($"[General/{message.Severity}] {message}");
            return Task.CompletedTask;
        }

        public Task Ready()
        {
            return interactionService.RegisterCommandsGloballyAsync();
        }

        public Task InteractionCreated(SocketInteraction interaction)
        {
            var ctx = new SocketInteractionContext(client, interaction);
            return interactionService.ExecuteCommandAsync(ctx, serviceProvider);
        }
    }
}
