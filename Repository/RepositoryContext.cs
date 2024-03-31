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

        public DbSet<PostTemplate>? PostTemplates { get; set; }

        public DbSet<Reaction>? Reactions { get; set; }

        public DbSet<User>? User { get; set; }
    }
}