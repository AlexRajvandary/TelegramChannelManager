using Telegram.Bot;
using TelegramChannelManager.Server.Extensions;
using TelegramChannelManager.Server.Services;
using TelegramChannelManager.Server.Services.UpdateHandlers;

namespace TelegramChannelManager.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
            builder.Services.Configure<BotConfiguration>(botConfigurationSection);
            var botConfiguration = botConfigurationSection.Get<BotConfiguration>();
            builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

            builder.Services.AddSingleton<IUserContextManager, UserContextManager>();
            builder.Services.AddScoped<MainBotHandlers>();
            builder.Services.AddScoped<CustomerBotHandlers>();
            builder.Services.AddHostedService<ConfigureWebhook>();
            builder.Services.ConfigureRepositoryManager();
            //builder.Services.ConfigureSqlContext(builder.Configuration);

            builder.Services
            .AddControllers()
            .AddNewtonsoftJson();

            var app = builder.Build();

            app.MapControllers();
            app.Run();
        }
    }
}
