using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Models;
using Repository.MySql;
using Service.Interfaces;
using Shared;
using Shared.DTOs.Vessel;
using Shared.Exceptions;

namespace Service.MySql.Vessel;

public class VesselMySqlService(VesselMySqlRepository vesselMySqlRepository, OwnerMySqlRepository ownerMySqlRepository) : IVesselService
{
    /*public async Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery = "")
    {
        return await dbContext.Vessels
            .Where(v => string.IsNullOrEmpty(searchQuery) || v.Name.StartsWith(searchQuery))
            .Include(v => v.Owner)
            .Select(v => new VesselResponse
            {
                Id = v.Id.ToString(),
                Name = v.Name,
                ImoNumber = v.ImoNumber,
                Flag = v.Flag,
                Owner = new OwnerResponse
                {
                    Id = v.Owner.Id.ToString(),
                    Name = v.Owner.Name
                }
            })
            .ToListAsync();
    }*/
    
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
    public async Task<VesselResponse> GetVesselById(string vesselId)
    {
        var vessel = await vesselMySqlRepository.GetVesselByIdAsync(vesselId);
        
        if (vessel == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");

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

    public async Task<VesselResponse> CreateVessel(VesselRequest vesselRequest)
    {
        var owner = await ownerMySqlRepository.GetOwnerByIdAsync(vesselRequest.OwnerId);
        if (owner == null)
            throw new NotFoundException($"Owner with id '{vesselRequest.OwnerId}' not found");

        var vessel = new Domain.Models.Vessel
        {
            Name = vesselRequest.Name,
            ImoNumber = vesselRequest.ImoNumber,
            Flag = vesselRequest.Flag,
            Owner = owner
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
        var vessel = await vesselMySqlRepository.GetVesselByIdAsync(vesselId);
        if (vessel == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");
        
        var owner = await ownerMySqlRepository.GetOwnerByIdAsync(vesselRequest.OwnerId);
        if (owner == null)
            throw new NotFoundException($"Owner with id '{vesselRequest.OwnerId}' not found");
        
        vessel.Name = vesselRequest.Name;
        vessel.ImoNumber = vesselRequest.ImoNumber;
        vessel.Flag = vesselRequest.Flag;
        vessel.Owner = owner;
        
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
        throw new NotImplementedException();
    }

}
