using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using EncurtadorUrl.Models;

namespace EncurtadorUrl.Data.Common
{
    public class AppDbContext : DbContext
    {

        public DbSet<UrlModel> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }    

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
