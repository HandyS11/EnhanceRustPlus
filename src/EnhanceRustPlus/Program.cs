using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using EnhanceRustPlus.Configuration;
using EnhanceRustPlus.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EnhanceRustPlus
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        private static ServiceProvider _serviceProvider = null!;
        private static DiscordSocketClient _client = null!;

        public static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting up");

                _client = new DiscordSocketClient(new DiscordSocketConfig()
                {
                    UseInteractionSnowflakeDate = false
                });

                var interactionService = new InteractionService(_client.Rest);

                var builder = new HostBuilder()
                    .ConfigureAppConfiguration((_, config) =>
                    {
                        config.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
                    })
                    .ConfigureServices((context, services) =>
                    {
                        var botSettings = new BotSettings();
                        context.Configuration.Bind(botSettings);

                        services.AddSingleton(_client);
                        services.AddSingleton(interactionService);
                        services.AddSingleton(botSettings);
                        services.AddSingleton<EventHandlers>();

                        services.AddBusinessServices();
                        services.AddDbContext(context.HostingEnvironment.IsDevelopment());
                    })
                    .UseSerilog();

                var host = builder.Build();
                _serviceProvider = (ServiceProvider)host.Services;

                var config = _serviceProvider.GetRequiredService<BotSettings>();

                await _client.LoginAsync(TokenType.Bot, config.DiscordBotToken);
                await _client.StartAsync();

                await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
                AttachEventHandlers();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred during startup");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }

        private static void AttachEventHandlers()
        {
            var eventHandlers = _serviceProvider.GetRequiredService<EventHandlers>();

            _client.Log += EventHandlers.LogAsync;
            _client.Ready += eventHandlers.Ready;
            _client.InteractionCreated += eventHandlers.InteractionCreated;
        }
    }
}