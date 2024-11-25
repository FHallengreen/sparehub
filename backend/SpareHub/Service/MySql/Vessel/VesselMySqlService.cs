using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Repository.MySql;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Vessel;
using Shared.Exceptions;

namespace Service.MySql.Vessel;

public class VesselMySqlService(VesselMySqlRepository vesselMySqlRepository) : IVesselService
{
    public async Task<List<VesselResponse>> GetVessels()
    {
        var vessels = await vesselMySqlRepository.GetVesselsAsync();

        if (vessels == null || vessels.Count == 0)
            throw new NotFoundException("No vessels found");

        return vessels.Select(v => new VesselResponse
        {
            Id = v.Id,
            Name = v.Name,
            ImoNumber = v.ImoNumber,
            Flag = v.Flag,
            Owner = new OwnerResponse
            {
                Id = v.Owner.Id,
                Name = v.Owner.Name
            }
        }).ToList();
    }
    public async Task<VesselRequest> GetVesselById(string vesselId)
    {
        var vessel = await vesselMySqlRepository.GetVesselByIdAsync(vesselId);
        
        if (vessel == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");

        return new VesselRequest
        {
            Id = vessel.Id,
            Name = vessel.Name,
            ImoNumber = vessel.ImoNumber,
            Flag = vessel.Flag,
            Owner = new OwnerRequest
            {
                Id = vessel.Owner.Id,
                Name = vessel.Owner.Name
            }
        };
    }

    public async Task<VesselResponse> CreateVessel(VesselRequest vesselRequest)
    {
        
        var vessel = new Domain.Models.Vessel
        {
            Id = vesselRequest.Id,
            Name = vesselRequest.Name,
            ImoNumber = vesselRequest.ImoNumber,
            Flag = vesselRequest.Flag,
            Owner = new Owner
            {
                Id = vesselRequest.Owner.Id,
                Name = vesselRequest.Owner.Name
            }
        };

        var createdVessel = await vesselMySqlRepository.CreateVesselAsync(vessel);

        return new VesselResponse
        {
            Id = createdVessel.Id,
            Name = createdVessel.Name,
            ImoNumber = createdVessel.ImoNumber,
            Flag = createdVessel.Flag,
            Owner = new OwnerResponse
            {
                Id = createdVessel.Owner.Id,
                Name = createdVessel.Owner.Name
            }
        };
    }

    public async Task<VesselResponse> UpdateVessel(string vesselId, VesselRequest vesselRequest)
    {
        if (string.IsNullOrEmpty(vesselId))
            throw new ValidationException("Vessel Id cannot be null or empty");

        var vessel = new Domain.Models.Vessel
        {
            Id = vesselId,
            Name = vesselRequest.Name,
            ImoNumber = vesselRequest.ImoNumber,
            Flag = vesselRequest.Flag,
            Owner = new Owner
            {
                Id = vesselRequest.Owner.Id,
                Name = vesselRequest.Owner.Name
            }
        };

        await vesselMySqlRepository.UpdateVesselAsync(vesselId, vessel);

        return new VesselResponse
        {
            Id = vessel.Id,
            Name = vessel.Name,
            ImoNumber = vessel.ImoNumber,
            Flag = vessel.Flag,
            Owner = new OwnerResponse
            {
                Id = vessel.Owner.Id,
                Name = vessel.Owner.Name
            }
        };
    }

    public async Task DeleteVessel(string vesselId)
    {
        if (string.IsNullOrEmpty(vesselId))
            throw new ValidationException("Vessel Id cannot be null or empty");

        await vesselMySqlRepository.DeleteVesselAsync(vesselId);
    }
}
