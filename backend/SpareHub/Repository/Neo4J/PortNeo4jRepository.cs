using Domain.Models;
using Neo4j.Driver;
using Repository.Interfaces;
using Shared.DTOs.Port;
using Shared.Exceptions;

namespace Repository.Neo4J;

public class PortNeo4jRepository(IDriver driver) : IPortRepository
{

    public async Task<List<PortResponse>> GetPortsBySearchQueryAsync(string? searchQuery = "")
    {
        await using var session = driver.AsyncSession();
        
        var query = @"
            WHERE $searchQuery IS NULL OR v.name STARTS WITH $searchQuery
            RETURN v.id as id, v.name as name";
    
        var result = await session.RunAsync(query, new { searchQuery });
        var ports = await result.ToListAsync(record => new PortResponse
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>()
        });
        return ports;
    }
    public async Task<Port> CreatePortAsync(Port port)
    {
        await using var session = driver.AsyncSession();

        // Ensure the ID is generated if not provided
        if (string.IsNullOrEmpty(port.Id))
        {
            port = new Port
            {
                Id = Guid.NewGuid().ToString(),
                Name = port.Name,
            };
        }

        var query = @"
            CREATE (p:Port {
                id: $id,
                name: $name
            })
            RETURN p.id as id, p.name as name";

        var parameters = new
        {
            id = port.Id,
            name = port.Name
        };

        var result = await session.RunAsync(query, parameters);
        var record = await result.SingleAsync();

        return new Port
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>()
        };
    }

    public async Task<List<Port>> GetPortsAsync()
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (p:Port)
            RETURN p.id as id, p.name as name";

        var result = await session.RunAsync(query);
        var ports = await result.ToListAsync(record => new Port
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>()
        });

        return ports;
    }

    public async Task<Port> GetPortByIdAsync(string portId)
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (p:Port)
            WHERE p.id = $portId
            RETURN p.id as id, p.name as name";

        var result = await session.RunAsync(query, new { portId });
        var record = await result.SingleAsync();

        
        return new Port
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>()
        };

    }

    public async Task UpdatePortAsync(string portId, Port port)
    {
        await using var session = driver.AsyncSession();

        // First check if port exists
        var checkQuery = @"
            MATCH (p:Port {id: $portId})
            RETURN p";

        var checkResult = await session.RunAsync(checkQuery, new { portId });
        if (!await checkResult.FetchAsync())
        {
            throw new NotFoundException($"Port with id '{portId}' not found");
        }

        var query = @"
            MATCH (p:Port {id: $portId})
            SET p.name = $name,
            RETURN p.id as id";

        var parameters = new
        {
            portId = port.Id,
            name = port.Name
        };

        await session.RunAsync(query, parameters);
    }

    public async Task DeletePortAsync(string portId)
    {
        await using var session = driver.AsyncSession();

        // First check if port exists
        var checkQuery = @"
            MATCH (p:Port {id: $portId})
            RETURN p";

        var checkResult = await session.RunAsync(checkQuery, new { portId });
        if (!await checkResult.FetchAsync())
        {
            throw new NotFoundException($"Port with id '{portId}' not found");
        }

        var query = @"
            MATCH (p:Port {id: $portId})
            DETACH DELETE p";

        await session.RunAsync(query, new { portId });
    }
}