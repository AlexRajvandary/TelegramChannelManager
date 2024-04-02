using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Service;
using Service.Contracts;

namespace ChannelManager.API.Extensions
{
    public static class ServiceExtensions
    {
        public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            var o = serviceProvider.GetService<IOptions<T>>();
            if (o is null)
                throw new ArgumentNullException(nameof(T));

            return o.Value;
        }

        public static ControllerActionEndpointConventionBuilder MapBotWebhookRoute<T>(
            this IEndpointRouteBuilder endpoints,
            string route)
        {
            var controllerName = typeof(T).Name.Replace("Controller", "", StringComparison.Ordinal);
            var actionName = typeof(T).GetMethods()[0].Name;

            return endpoints.MapControllerRoute(
                name: "bot_webhook",
                pattern: route,
                defaults: new { controller = controllerName, action = actionName });
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
                    services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
                   services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureSqlContext(this IServiceCollection services,
                    IConfiguration configuration) =>
                        services.AddDbContext<RepositoryContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("sqlConnection")));

    }
}
