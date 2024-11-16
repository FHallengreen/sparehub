using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.MySql.Order;

namespace Service;

public class DatabaseFactory(IServiceProvider serviceProvider, IOptions<DatabaseSettings> databaseSettings)
    : IDatabaseFactory
{
    private readonly DatabaseSettings _databaseSettings = databaseSettings.Value;
        private readonly Dictionary<(Type serviceType, DatabaseType dbType), Type> _serviceMappings = new()
        {
            { (typeof(IBoxService), DatabaseType.MySql), typeof(BoxMySqlService) },
            /*{ (typeof(IBoxService), DatabaseType.MongoDb), typeof(BoxMongoDbService) },
            { (typeof(IBoxService), DatabaseType.Neo4j), typeof(BoxNeo4jService) },*/
            { (typeof(IOrderService), DatabaseType.MySql), typeof(OrderMySqlService) },
            // { (typeof(IOrderService), DatabaseType.MongoDb), typeof(OrderMongoDbService) },
        };

        // Add other service mappings as needed

        public T GetService<T>() where T : class
        {
            var serviceType = typeof(T);
            var dbType = _databaseSettings.DefaultDatabaseType;

            if (_serviceMappings.TryGetValue((serviceType, dbType), out var implementationType))
            {
                var service = serviceProvider.GetService(implementationType) as T;
                if (service == null)
                {
                    throw new InvalidOperationException($"Failed to resolve service '{implementationType.Name}'. Ensure it is registered.");
                }
                return service;
            }
            else
            {
                throw new InvalidOperationException($"No service mapping found for type '{serviceType.Name}' and database '{dbType}'.");
            }
        }
    }
