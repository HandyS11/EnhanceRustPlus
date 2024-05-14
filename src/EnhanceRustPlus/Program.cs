using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using EnhanceRustPlus.Configuration;
using EnhanceRustPlus.Setup;
using EnhanceRustPlus.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EnhanceRustPlus
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        private static ServiceProvider _serviceProvider = null!;
        private static DiscordSocketClient _client = null!;

        public static async Task Main()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                UseInteractionSnowflakeDate = false
            });

            var interactionService = new InteractionService(_client.Rest);

            var builder = Host.CreateApplicationBuilder();
            var config = builder.Configuration.Get<Config>()
                         ?? throw new FileNotFoundException("Failed to load a valid config from AppSettings.json");

            _serviceProvider = builder.Services
                .AddSingleton(interactionService)
                .AddSingleton(_client)
                .AddSingleton<EventHandlers>()
                .BuildServiceProvider();

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();

            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
            AttachEventHandlers();

            _client.Ready += async () =>
            {
                var setupApp = _serviceProvider.GetRequiredService<SetupApp>();
                await setupApp.SetupCategory(1237384106346680361);
            };

            var app = builder.Build();
            await app.RunAsync();
        }

        private static void AttachEventHandlers()
        {
            var eventHandlers = _serviceProvider.GetRequiredService<EventHandlers>();

            _client.Log += EventHandlers.LogMessage;
            _client.Ready += eventHandlers.Ready;
            _client.InteractionCreated += eventHandlers.InteractionCreated;
        }
    }
}