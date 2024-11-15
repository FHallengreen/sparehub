using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Service;

public class DatabaseFactory(IServiceProvider serviceProvider, IOptions<DatabaseSettings> databaseSettings)
    : IDatabaseFactory
{
    private readonly DatabaseSettings _databaseSettings = databaseSettings.Value;

    public T GetService<T>() where T : class
    {
        return _databaseSettings.DefaultDatabaseType switch
        {
            DatabaseType.MySql => ResolveService<T>("MySqlService"),
            DatabaseType.MongoDb => ResolveService<T>("MongoDbService"),
            DatabaseType.Neo4j => ResolveService<T>("Neo4jService"),
            _ => throw new ArgumentException("Invalid database type"),
        };
    }

    private T ResolveService<T>(string serviceSuffix) where T : class
    {
        var interfaceName = typeof(T).Name; // e.g., "IBoxService"
        var expectedServiceName = interfaceName.Substring(1).Replace("Service", "") + serviceSuffix; // e.g., "BoxMySqlService"

        var serviceType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name.Equals(expectedServiceName, StringComparison.OrdinalIgnoreCase));

        if (serviceType == null)
        {
            throw new InvalidOperationException($"Service type '{expectedServiceName}' not found.");
        }

        var resolvedService = serviceProvider.GetService(serviceType);
        if (resolvedService == null)
        {
            throw new InvalidOperationException($"Failed to resolve service '{expectedServiceName}'. Ensure it is registered.");
        }

        return resolvedService as T;
    }

}