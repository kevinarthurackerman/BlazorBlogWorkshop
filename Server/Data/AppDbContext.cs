using BlazorBlogWorkshop.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlogWorkshop.Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
