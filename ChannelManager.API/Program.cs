using ChannelManager.API.Extensions;
using ChannelManager.API.Services.BotHandlers;
using ChannelManager.API.Services;
using ChannelManager.API;
using Telegram.Bot;

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