using EncurtadorUrl.Data.Common;
using EncurtadorUrl.Interfaces;

namespace EncurtadorUrl.Configuration
{
    public static class CreateDatabaseConfig
    {
        public static async Task InformacoesIniciaisAsync(WebApplication app)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "jsonUrls.json");
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                var service = scope.ServiceProvider.GetRequiredService<IUrlService>();
                var quewe = scope.ServiceProvider.GetRequiredService<IRabbitMqClient>();
                var repo = scope.ServiceProvider.GetRequiredService<IUrlRepository>();

                if (File.Exists(filePath))
                {
                    var listUrls =  await service.ProcessFile(filePath);
                    foreach (var url in listUrls)
                    {
                        repo.CreateUrl(url);
                        quewe.PublicUrl(url);
                    }
                }

            }
        }       

    }
}
