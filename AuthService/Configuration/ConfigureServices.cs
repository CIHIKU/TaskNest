using AuthService.Services;
using AuthService.Services.Interfaces;
using Microsoft.OpenApi.Models;

namespace AuthService.Configuration;

public static class ConfigureServices
{
    public static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqPublisherService, RabbitMqPublisherService>();
        services.AddSingleton<IRabbitMqConsumerService, RabbitMqConsumerService>();

        services.AddControllers();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Auth Service",
            Description = "Auth Service for TaskNest",
            Version = "v1",
        }));
    }
}