using Microsoft.EntityFrameworkCore;
using TelegramChannelManager.Server.Models;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Post>? Posts { get; set; }
    }
}
