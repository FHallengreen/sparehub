using Domain.Models;
using Neo4j.Driver;
using Repository.Interfaces;
using Shared.DTOs.Owner;
using Shared.DTOs.Vessel;
using Shared.Exceptions;

namespace Repository.Neo4J;

public class VesselNeo4jRepository(IDriver driver) : IVesselRepository
{
    public async Task<List<VesselResponse>> GetVesselsBySearchQueryAsync(string? searchQuery = "")
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (v:Vessel)-[:OWNED_BY]->(o:Owner)
            WHERE $searchQuery IS NULL OR v.name STARTS WITH $searchQuery
            RETURN v.id as id, v.name as name, v.imoNumber as imoNumber, 
                   v.flag as flag, o.id as ownerId, o.name as ownerName";

        var result = await session.RunAsync(query, new { searchQuery });
        var vessels = await result.ToListAsync(record => new VesselResponse
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>(),
            ImoNumber = record["imoNumber"].As<string>(),
            Flag = record["flag"].As<string>(),
            Owner = new OwnerResponse
            {
                Id = record["ownerId"].As<string>(),
                Name = record["ownerName"].As<string>()
            }
        });

        return vessels;
    }

    public async Task<List<Vessel>> GetVesselsAsync()
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (v:Vessel)-[:OWNED_BY]->(o:Owner)
            RETURN v.id as id, v.name as name, v.imoNumber as imoNumber, 
                   v.flag as flag, o.id as ownerId, o.name as ownerName
            ORDER BY v.id";

        var result = await session.RunAsync(query);
        var vessels = await result.ToListAsync(record => new Vessel
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>(),
            ImoNumber = record["imoNumber"].As<string>(),
            Flag = record["flag"].As<string>(),
            Owner = new Owner
            {
                Id = record["ownerId"].As<string>(),
                Name = record["ownerName"].As<string>()
            }
        });

        return vessels;
    }

    public async Task<Vessel> GetVesselByIdAsync(string vesselId)
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (v:Vessel)-[:OWNED_BY]->(o:Owner)
            WHERE v.id = $vesselId
            RETURN v.id as id, v.name as name, v.imoNumber as imoNumber, 
                   v.flag as flag, o.id as ownerId, o.name as ownerName";

        var result = await session.RunAsync(query, new { vesselId });
        var record = await result.SingleAsync();

        return new Vessel
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>(),
            ImoNumber = record["imoNumber"].As<string>(),
            Flag = record["flag"].As<string>(),
            Owner = new Owner
            {
                Id = record["ownerId"].As<string>(),
                Name = record["ownerName"].As<string>()
            }
        };
    }

    public async Task<Vessel> CreateVesselAsync(Vessel vessel)
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (o:Owner {id: $ownerId})
            CREATE (v:Vessel {
                id: $id,
                name: $name,
                imoNumber: $imoNumber,
                flag: $flag
            })-[:OWNED_BY]->(o)
            RETURN v.id as id, v.name as name, v.imoNumber as imoNumber, 
                   v.flag as flag, o.id as ownerId, o.name as ownerName";

        var parameters = new
        {
            id = vessel.Id,
            name = vessel.Name,
            imoNumber = vessel.ImoNumber,
            flag = vessel.Flag,
            ownerId = vessel.Owner.Id
        };

        var result = await session.RunAsync(query, parameters);
        var record = await result.SingleAsync();

        return new Vessel
        {
            Id = record["id"].As<string>(),
            Name = record["name"].As<string>(),
            ImoNumber = record["imoNumber"].As<string>(),
            Flag = record["flag"].As<string>(),
            Owner = new Owner
            {
                Id = record["ownerId"].As<string>(),
                Name = record["ownerName"].As<string>()
            }
        };
    }

    public async Task UpdateVesselAsync(string vesselId, Vessel vessel)
    {
        await using var session = driver.AsyncSession();

        // First check if vessel exists
        var checkQuery = @"
            MATCH (v:Vessel {id: $vesselId})
            RETURN v";

        var checkResult = await session.RunAsync(checkQuery, new { vesselId });
        if (!await checkResult.FetchAsync())
        {
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");
        }

        var query = @"
            MATCH (v:Vessel {id: $vesselId})
            MATCH (o:Owner {id: $ownerId})
            MATCH (v)-[r:OWNED_BY]->(:Owner)
            DELETE r
            CREATE (v)-[:OWNED_BY]->(o)
            SET v.name = $name,
                v.imoNumber = $imoNumber,
                v.flag = $flag
            RETURN v.id as id";

        var parameters = new
        {
            vesselId = vessel.Id,
            name = vessel.Name,
            imoNumber = vessel.ImoNumber,
            flag = vessel.Flag,
            ownerId = vessel.Owner.Id
        };

        await session.RunAsync(query, parameters);
    }

    public async Task DeleteVesselAsync(string vesselId)
    {
        await using var session = driver.AsyncSession();

        // First check if vessel exists
        var checkQuery = @"
            MATCH (v:Vessel {id: $vesselId})
            RETURN v";

        var checkResult = await session.RunAsync(checkQuery, new { vesselId });
        if (!await checkResult.FetchAsync())
        {
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");
        }

        var query = @"
            MATCH (v:Vessel {id: $vesselId})
            DETACH DELETE v";

        await session.RunAsync(query, new { vesselId });
    }
}