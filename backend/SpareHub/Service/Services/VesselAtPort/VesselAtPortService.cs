﻿using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.Owner;
using Shared.DTOs.Vessel;
using Shared.DTOs.VesselAtPort;
using Shared.Exceptions;


namespace Service.Services.VesselAtPort;

public class VesselAtPortService(IVesselAtPortRepository vesselAtPortRepository) : IVesselAtPortService
{

    public async Task<List<VesselAtPortResponse>> GetVesselAtPorts(IVesselRepository vesselRepository)
    {
        var vesselsAtPorts = await vesselAtPortRepository.GetVesselAtPortAsync();
        var vesselAtPortResponses = new List<VesselAtPortResponse>();
        foreach (var vesselAtPort in vesselsAtPorts)
        {
            var vessel = await vesselRepository.GetVesselByIdAsync(vesselAtPort.VesselId);
            var vesselAtPortResponse = new VesselAtPortResponse
            {
                PortId = vesselAtPort.PortId,
                Vessels = new List<VesselResponse>
                {
                    new VesselResponse
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
                    }
                }
            };
            vesselAtPortResponses.Add(vesselAtPortResponse);
        }
        return vesselAtPortResponses;
    }

    public async Task<VesselAtPortResponse> GetVesselByIdAtPort(string vesselId, IVesselRepository vesselRepository)
    {
        var vesselAtPort = await vesselAtPortRepository.GetVesselByIdAtPortAsync(vesselId);
        var vessel = await vesselRepository.GetVesselByIdAsync(vesselAtPort.VesselId);
        return new VesselAtPortResponse
        {
            PortId = vesselAtPort.PortId,
            Vessels = new List<VesselResponse>
            {
                new VesselResponse
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
                }
            }
        };
    }

    public async Task<VesselAtPortResponse> AddVesselToPort(VesselAtPortRequest vesselAtPortRequest, IVesselRepository vesselRepository)
    {
        var vessel = await vesselRepository.GetVesselByIdAsync(vesselAtPortRequest.VesselId);
        if (vessel == null)
            throw new NotFoundException($"Vessel with id '{vesselAtPortRequest.VesselId}' not found");
        
        var vesselAtPort = new Domain.Models.VesselAtPort
        {
            PortId = vesselAtPortRequest.PortId,
            VesselId = vesselAtPortRequest.VesselId
        };
        
        var createdVesselAtPort = await vesselAtPortRepository.AddVesselToPortAsync(vesselAtPort);
        
        return new VesselAtPortResponse
        {
            PortId = createdVesselAtPort.PortId,
            Vessels = new List<VesselResponse>
            {
                new VesselResponse
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
                }
            }
        };
    }

    public async Task<VesselAtPortResponse> ChangePortForVesselAsync(VesselAtPortRequest vesselAtPortRequest,
        IVesselRepository vesselRepository)
    {
        var vesselAtPort = await vesselAtPortRepository.GetVesselByIdAtPortAsync(vesselAtPortRequest.VesselId);
        if (vesselAtPort == null)
            throw new NotFoundException($"Vessel with id '{vesselAtPortRequest.VesselId}' not found at any port");

        await RemoveVesselFromPort(vesselAtPortRequest.VesselId);
        return await AddVesselToPort(vesselAtPortRequest, vesselRepository);
    }

    public async Task RemoveVesselFromPort(string vesselId)
    {
        var vesselAtPort = await vesselAtPortRepository.GetVesselByIdAtPortAsync(vesselId);
        if (vesselAtPort == null)
            throw new NotFoundException($"Vessel with id '{vesselId}' not found at any port");
        
        await vesselAtPortRepository.RemoveVesselFromPortAsync(vesselId);
    }
}