using Domain.Models;
using Neo4j.Driver;
using Repository.Interfaces;
using Shared.DTOs.VesselAtPort;
using Shared.Exceptions;
using AutoMapper;

namespace Repository.Neo4J;

public class VesselAtPortNeo4jRepository(IDriver driver, IMapper mapper) : IVesselAtPortRepository
{
    public async Task<List<VesselAtPort>> GetVesselAtPortAsync()
    {
        await using var session = driver.AsyncSession();

        var query = @"
            MATCH (v:Vessel)-[:DOCKED_AT]->(p:Port)
            RETURN v.id as id, v.name as name, v.imo as imo, 
                   v.type as type, p.id as portId";

        var result = await session.RunAsync(query);
        var vessels = await result.ToListAsync(record =>
            new VesselAtPort
            {
                VesselId = record["id"].As<string>(),
                PortId = record["portId"].As<string>(),
                ArrivalDate = null,
                DepartureDate = null
            });

        return vessels;
    }

    public async Task<VesselAtPort> GetVesselByIdAtPortAsync(string vesselId)
    {
        if (string.IsNullOrEmpty(vesselId))
            throw new ArgumentNullException(nameof(vesselId));

        await using var session = driver.AsyncSession();
        
        var query = @"
            MATCH (v:Vessel)-[:DOCKED_AT]->(p:Port)
            WHERE v.id = $vesselId
            RETURN v.id as id, v.name as name, v.imo as imo, 
                   v.type as type, p.id as portId";

        var result = await session.RunAsync(query, new { vesselId });
        var record = await result.SingleAsync();

        return new VesselAtPort
        {
            VesselId = record["id"].As<string>(),
            PortId = record["portId"].As<string>(),
            ArrivalDate = null,
            DepartureDate = null
        };
    }

    public async Task<VesselAtPort> AddVesselToPortAsync(VesselAtPort vesselAtPort)
    {
        await using var session = driver.AsyncSession();

        if (vesselAtPort == null)
            throw new ArgumentNullException(nameof(vesselAtPort));

        // Check if the vessel exists
        var checkVesselQuery = @"
            MATCH (v:Vessel {id: $vesselId})
            RETURN v";

        var checkVesselResult = await session.RunAsync(checkVesselQuery, new { vesselId = vesselAtPort.VesselId });
        if (!await checkVesselResult.FetchAsync())
        {
            throw new NotFoundException($"Vessel with id '{vesselAtPort.VesselId}' not found");
        }

        // Check if the port exists
        var checkPortQuery = @"
            MATCH (p:Port {id: $portId})
            RETURN p";

        var checkPortResult = await session.RunAsync(checkPortQuery, new { portId = vesselAtPort.PortId });
        if (!await checkPortResult.FetchAsync())
        {
            throw new NotFoundException($"Port with id '{vesselAtPort.PortId}' not found");
        }

        // Create the relationship
        var query = @"
            MATCH (v:Vessel {id: $vesselId})
            MATCH (p:Port {id: $portId})
            MERGE (v)-[r:DOCKED_AT]->(p)
            RETURN v.id as vesselId, p.id as portId";

        var parameters = new
        {
            portId = vesselAtPort.PortId,
            vesselId = vesselAtPort.VesselId
        };

        var result = await session.RunAsync(query, parameters);
        var record = await result.SingleAsync();

        return new VesselAtPort
        {
            VesselId = record["vesselId"].As<string>(),
            PortId = record["portId"].As<string>(),
            ArrivalDate = vesselAtPort.ArrivalDate,
            DepartureDate = vesselAtPort.DepartureDate
        };
    }

    public async Task<VesselAtPortResponse> ChangePortForVesselAsync(VesselAtPort vesselAtPort, string newPortId)
    {
        if (vesselAtPort == null)
            throw new ArgumentNullException(nameof(vesselAtPort));

        // Remove the vessel from the current port
        await RemoveVesselFromPortAsync(vesselAtPort.VesselId);

        // Update the PortId to the new port
        vesselAtPort.PortId = newPortId;

        // Add the vessel to the new port
        var updatedVesselAtPort = await AddVesselToPortAsync(vesselAtPort);

        // Map and return the response
        return mapper.Map<VesselAtPortResponse>(updatedVesselAtPort);
    }

    public async Task RemoveVesselFromPortAsync(string vesselId)
    {
        if (string.IsNullOrEmpty(vesselId))
            throw new ArgumentNullException(nameof(vesselId));

        await using var session = driver.AsyncSession();

        // First check if vessel exists
        var checkQuery = @"
            MATCH (v:Vessel {id: $vesselId})
            RETURN v";

        var checkResult = await session.RunAsync(checkQuery, new { vesselId });
        if (!await checkResult.FetchAsync())
        {
            throw new NotFoundException($"Vessel with id '{vesselId}' not found at port");
        }

        var query = @"
            MATCH (v:Vessel)-[r:DOCKED_AT]->(:Port)
            WHERE v.id = $vesselId
            DELETE r, v";

        await session.RunAsync(query, new { vesselId });
    }
}