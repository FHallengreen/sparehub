using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.Owner;
using Shared.DTOs.Vessel;
using Shared.Exceptions;

namespace Service.Services.Vessel;

public class VesselService(IVesselRepository vesselRepository,IOwnerRepository ownerRepository) : IVesselService
{

    public async Task<List<VesselResponse>> GetVesselsBySearchQuery(string? searchQuery = "")
    {
        return await vesselRepository.GetVesselsBySearchQueryAsync(searchQuery);
    }
    
    public async Task<List<VesselResponse>> GetVessels()
    {
        var vessels = await vesselRepository.GetVesselsAsync();

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
        var vessel = await vesselRepository.GetVesselByIdAsync(vesselId);
        
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
        if (vesselRequest == null)
        {
            throw new ArgumentNullException(nameof(vesselRequest), "VesselRequest cannot be null.");
        }

        var owner = await ownerRepository.GetOwnerByIdAsync(vesselRequest.OwnerId);
        if (owner == null)
            throw new NotFoundException($"Owner with id '{vesselRequest.OwnerId}' not found");

        var vessel = new Domain.Models.Vessel
        {
            Name = vesselRequest.Name,
            ImoNumber = vesselRequest.ImoNumber,
            Flag = vesselRequest.Flag,
            Owner = owner
        };

        var createdVessel = await vesselRepository.CreateVesselAsync(vessel);

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
        var vessel = await vesselRepository.GetVesselByIdAsync(vesselId);
        if (vessel == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");
        
        var owner = await ownerRepository.GetOwnerByIdAsync(vesselRequest.OwnerId);
        if (owner == null)
            throw new NotFoundException($"Owner with id '{vesselRequest.OwnerId}' not found");
        
        vessel.Name = vesselRequest.Name;
        if (vesselRequest.ImoNumber != null) vessel.ImoNumber = vesselRequest.ImoNumber;
        if (vesselRequest.Flag != null) vessel.Flag = vesselRequest.Flag;
        vessel.Owner = owner;
        
        await vesselRepository.UpdateVesselAsync(vesselId, vessel);
        
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
        var vessel = await vesselRepository.GetVesselByIdAsync(vesselId);
        if (vessel == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found");
        
        await vesselRepository.DeleteVesselAsync(vesselId);
    }
    
    
    

}