using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace ChannelManager.API.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<RepositoryContext>().UseNpgsql(configuration.GetConnectionString("sqlConnection"), b => b.MigrationsAssembly("ChannelManager.API"));
            
            return new RepositoryContext(builder.Options);
        }
    }
}
