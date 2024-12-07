using Domain.Models;
using Neo4j.Driver;
using Persistence.Neo4j;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.Neo4J;

public class OwnerNeo4jRepository(IDriver driver) : IOwnerRepository
{
    public async Task<List<Owner>> GetOwnersAsync()
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (o:Owner)
            RETURN o.id as id, o.name as name";

        var result = await session.RunAsync(query);
        var owners = await result.ToListAsync(record => new Owner
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>()
        });

        return owners;
    }

    public async Task<Owner> GetOwnerByIdAsync(string ownerId)
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (o:Owner)
            WHERE o.id = $ownerId
            RETURN o.id as id, o.name as name";

        var result = await session.RunAsync(query, new { ownerId });
        var record = await result.SingleAsync();

        return new Owner
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>()
        };
    }

    public async Task<Owner> CreateOwnerAsync(Owner owner)
    {
        await using var session = driver.AsyncSession();

        var query = @"
            CREATE (o:Owner {
                id: $id,
                name: $name
            })
            RETURN o.id as id, o.name as name";

        var parameters = new
        {
            id = owner.Id,
            name = owner.Name
        };

        var result = await session.RunAsync(query, parameters);
        var record = await result.SingleAsync();

        return new Owner
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>()
        };
    }

    public async Task UpdateOwnerAsync(string ownerId, Owner owner)
    {
        await using var session = driver.AsyncSession();

        // First check if owner exists
        var checkQuery = @"
            MATCH (o:Owner {id: $ownerId})
            RETURN o";

        var checkResult = await session.RunAsync(checkQuery, new { ownerId });
        if (!await checkResult.FetchAsync())
        {
            throw new NotFoundException($"Owner with id '{ownerId}' not found");
        }

        var query = @"
            MATCH (o:Owner {id: $ownerId})
            SET o.name = $name
            RETURN o.id as id, o.name as name";

        var parameters = new
        {
            ownerId = owner.Id,
            name = owner.Name
        };

        await session.RunAsync(query, parameters);
    }

    public async Task DeleteOwnerAsync(string ownerId)
    {
        await using var session = driver.AsyncSession();

        // First check if owner exists
        var checkQuery = @"
            MATCH (o:Owner {id: $ownerId})
            RETURN o";

        var checkResult = await session.RunAsync(checkQuery, new { ownerId });
        if (!await checkResult.FetchAsync())
        {
            throw new NotFoundException($"Owner with id '{ownerId}' not found");
        }

        var query = @"
            MATCH (o:Owner {id: $ownerId})
            DELETE o";

        await session.RunAsync(query, new { ownerId });
    }
}