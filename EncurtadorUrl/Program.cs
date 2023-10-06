using EncurtadorUrl.Data.Common;
using EncurtadorUrl.Data.Repository;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Notificacoes;
using EncurtadorUrl.RabbitMqClient;
using EncurtadorUrl.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AppUrl");

var filePath = Path.Combine(Directory.GetCurrentDirectory(), "jsonUrls.json");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<INotificador, Notificador>();
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IUrlShortService, UrlShortService>();
builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Url - Api", Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

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
       var listUrls = await  service.ProcessFile(filePath);   
        foreach (var url in listUrls)
        {
            repo.CreateUrl(url);
            quewe.PublicUrl(url);
        }
    }
    
}

app.Run();
