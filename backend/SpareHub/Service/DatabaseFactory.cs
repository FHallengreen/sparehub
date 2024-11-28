using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.MySql.Dispatch;
using Service.MongoDb;
using Service.MySql;
using Service.MySql.Order;
using Service.MySql.Owner;
using Service.MySql.Port;
using Service.MySql.Vessel;
using Service.MySql.VesselAtPort;

namespace Service;

public class DatabaseFactory(IServiceProvider serviceProvider, IOptionsMonitor<DatabaseSettings> databaseSettings)
    : IDatabaseFactory
{
    private readonly Dictionary<(Type serviceType, DatabaseType dbType), Type> _serviceMappings = new()
    {
        { (typeof(IBoxService), DatabaseType.MySql), typeof(BoxMySqlService) },
        { (typeof(IBoxService), DatabaseType.MongoDb), typeof(BoxMongoDbService) },
        { (typeof(IOrderService), DatabaseType.MongoDb), typeof(OrderMongoDbService) },
        { (typeof(IOrderService), DatabaseType.MySql), typeof(OrderMySqlService) },
        { (typeof(IDispatchService), DatabaseType.MySql), typeof(DispatchMySqlService) },
        { (typeof(IDispatchService), DatabaseType.MongoDb), typeof(DispatchMongoDbService) },
        { (typeof(IPortService), DatabaseType.MySql), typeof(PortMySqlService) },
        { (typeof(IVesselService), DatabaseType.MySql), typeof(VesselMySqlService) },
        { (typeof(IOwnerService), DatabaseType.MySql), typeof(OwnerMySqlService) },
        { (typeof(IVesselAtPortService), DatabaseType.MySql), typeof(VesselAtPortMySqlService) }
    };

    // Add other service mappings as needed

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