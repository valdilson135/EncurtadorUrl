using EncurtadorUrl.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorUrl.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("AppUrl");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            return builder;
        }
    }
}
