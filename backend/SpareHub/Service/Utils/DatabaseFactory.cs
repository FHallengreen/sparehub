using Microsoft.Extensions.Options;
using Repository.Interfaces;
using Repository.MongoDb;
using Repository.MySql;
using Repository.Neo4J;
using Service.Interfaces;

namespace Service.Utils;

public class DatabaseFactory(IServiceProvider serviceProvider, IOptionsMonitor<DatabaseSettings> databaseSettings)
    : IDatabaseFactory
{
    private readonly Dictionary<(Type repositoryType, DatabaseType dbType), Type> _repositoryMappings = new()
    {
        //Box
        { (typeof(IBoxRepository), DatabaseType.MySql), typeof(BoxMySqlRepository) },
        { (typeof(IBoxRepository), DatabaseType.MongoDb), typeof(BoxMongoDbRepository) },

        //Order
        { (typeof(IOrderRepository), DatabaseType.MySql), typeof(OrderMySqlRepository) },
        { (typeof(IOrderRepository), DatabaseType.MongoDb), typeof(OrderMongoDbRepository) },

        //Dispatch
        { (typeof(IDispatchRepository), DatabaseType.MySql), typeof(DispatchMySqlRepository) },
        { (typeof(IDispatchRepository), DatabaseType.MongoDb), typeof(DispatchMongoDbRepository) },

        //Warehouse
        { (typeof(IWarehouseRepository), DatabaseType.MySql), typeof(WarehouseMySqlRepository) },

        //Agent
        { (typeof(IAgentRepository), DatabaseType.MySql), typeof(AgentMySqlRepository) },
        
        //Address
        { (typeof(IAddressRepository), DatabaseType.MySql), typeof(AddressMySqlRepository) },
        
        //Port
        { (typeof(IPortService), DatabaseType.MySql), typeof(PortMySqlRepository) },
        
        //Vessel
        { (typeof(IVesselService), DatabaseType.MySql), typeof(VesselMySqlRepository) },
        { (typeof(IVesselAtPortService), DatabaseType.MySql), typeof(VesselAtPortMySqlRepository) },

        //Owner
        { (typeof(IOwnerService), DatabaseType.MySql), typeof(OwnerMySqlRepository) },
        
        { (typeof(IPortRepository), DatabaseType.MySql), typeof(PortMySqlRepository) },
        { (typeof(IPortRepository), DatabaseType.Neo4j), typeof(PortNeo4jRepository)},
        { (typeof(IVesselRepository), DatabaseType.MySql), typeof(VesselMySqlRepository) },
        { (typeof(IVesselRepository), DatabaseType.Neo4j), typeof(VesselNeo4jRepository)},
        { (typeof(IOwnerRepository), DatabaseType.MySql), typeof(OwnerMySqlRepository) },
        { (typeof(IOwnerRepository), DatabaseType.Neo4j), typeof(OwnerNeo4jRepository)},
        { (typeof(IVesselAtPortRepository), DatabaseType.MySql), typeof(VesselAtPortMySqlRepository) },
        { (typeof(IVesselAtPortRepository), DatabaseType.Neo4j), typeof(VesselAtPortNeo4jRepository)}
    };

    public T GetRepository<T>() where T : class
    {
        var repositoryType = typeof(T);
        var dbType = databaseSettings.CurrentValue.DefaultDatabaseType;

        if (_repositoryMappings.TryGetValue((repositoryType, dbType), out var implementationType))
        {
            var repository = serviceProvider.GetService(implementationType) as T;
            if (repository == null)
            {
                throw new InvalidOperationException(
                    $"Failed to resolve repository '{implementationType.Name}'. Ensure it is registered.");
            }

            return repository;
        }

        throw new InvalidOperationException(
            $"No repository mapping found for type '{repositoryType.Name}' and database '{dbType}'.");
    }
}