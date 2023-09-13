using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace SharedLibrary.Extensions;

public static class MongoDbServiceExtensions
{
    public static IServiceCollection AddMongoDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;
        services.AddSingleton(mongoDbSettings);

        var mongoDbUri = Environment.GetEnvironmentVariable("MONGO_DB_URI") ?? mongoDbSettings.ConnectionString;
        if (string.IsNullOrEmpty(mongoDbUri)) throw new InvalidOperationException("MongoDB connection string is not configured.");

        services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoDbUri));

        services.AddScoped(sp => 
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDbSettings.DatabaseName);
        });

        return services;
    }
}

public class MongoDbSettings
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
}