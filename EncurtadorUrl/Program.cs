using EncurtadorUrl.Data.Common;
using EncurtadorUrl.Data.Repository;
using EncurtadorUrl.Data.Services;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Models;
using EncurtadorUrl.Notificacoes;
using EncurtadorUrl.RabbitMqClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

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

    if(File.Exists(filePath))
    {
        var contentFile = File.ReadAllText(filePath);
        var dadosFile = JsonConvert.DeserializeObject<List<UrlModel>>(contentFile);
        var service = new UrlRepository(db);
        var fila = new RabbitMqClient(builder.Configuration);

        foreach (var item in dadosFile)
        {
            var ret = await service.GetUrlByShortUrl(item);
            if (ret == null)
            {
                item.Id = 0;
                await service.CreateUrl(item);
                fila.PublicUrl(item);
            }
        }
    }
    
}

app.Run();
