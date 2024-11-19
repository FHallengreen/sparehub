using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.MongoDb;
using Service.MySql.Order;

namespace Service;

public class DatabaseFactory(IServiceProvider serviceProvider, IOptionsMonitor<DatabaseSettings> databaseSettings)
    : IDatabaseFactory
{
    private readonly Dictionary<(Type serviceType, DatabaseType dbType), Type> _serviceMappings = new()
    {
        { (typeof(IBoxService), DatabaseType.MySql), typeof(BoxMySqlService) },
        { (typeof(IOrderService), DatabaseType.MongoDb), typeof(OrderMongoDbService) },
        { (typeof(IOrderService), DatabaseType.MySql), typeof(OrderMySqlService) },
    };

    public T GetService<T>() where T : class
    {
        var serviceType = typeof(T);
        var dbType = databaseSettings.CurrentValue.DefaultDatabaseType;

        if (_serviceMappings.TryGetValue((serviceType, dbType), out var implementationType))
        {
            var service = serviceProvider.GetService(implementationType) as T;
            if (service == null)
            {
                throw new InvalidOperationException(
                    $"Failed to resolve service '{implementationType.Name}'. Ensure it is registered.");
            }

            return service;
        }
        else
        {
            throw new InvalidOperationException(
                $"No service mapping found for type '{serviceType.Name}' and database '{dbType}'.");
        }
    }
}