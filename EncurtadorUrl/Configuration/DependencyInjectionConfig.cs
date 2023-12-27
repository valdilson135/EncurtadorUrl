using EncurtadorUrl.Data.Repository;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Notificacoes;
using EncurtadorUrl.Services;
using EncurtadorUrl.RabbitMqClient;

namespace EncurtadorUrl.Configuration
{
    public static class DependencyInjectionConfig
    {
       public static WebApplicationBuilder AddInjectionConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<INotificador, Notificador>();
            builder.Services.AddScoped<IUrlRepository, UrlRepository>();
            builder.Services.AddScoped<IUrlService, UrlService>();
            builder.Services.AddScoped<IUrlShortService, UrlShortService>();
            builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClients>();

            //AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            return builder;
        }
    }
}
