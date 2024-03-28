using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Post>? Posts { get; set; }

        public DbSet<Post>? Reactions { get; set; }

        public DbSet<Post>? User { get; set; }
    }
}