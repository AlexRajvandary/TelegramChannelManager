using ChannelManager.API.Extensions;
using ChannelManager.API.Services.BotHandlers;
using ChannelManager.API.Services;
using ChannelManager.API;
using Telegram.Bot;
using Contracts;

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

builder.Services.ConfigureLoggerService();
builder.Services.AddSingleton<ITelegramClientsManager, TelegramClientsManager>();
builder.Services.AddScoped<MainBotHandlers>();
builder.Services.AddScoped<CustomerBotHandlers>();
builder.Services.AddHostedService<ConfigureWebhook>();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services
.AddControllers()
.AddNewtonsoftJson();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();

app.ConfigureExceptionHandler(logger);
if (app.Environment.IsProduction())
    app.UseHsts();

app.MapControllers();
app.Run();