using EncurtadorUrl.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorUrl.Data.Common
{
    public class AppDbContext : IdentityDbContext
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
