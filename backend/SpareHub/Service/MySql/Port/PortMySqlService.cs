using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Repository.MySql;
using Service.Interfaces;
using Shared.DTOs.Vessel;
using Shared.DTOs.Port;
using Shared.Exceptions;


namespace Service.MySql.Vessel;

    public class PortMySqlService(PortMySqlRepository portMySqlRepository) : IPortService
    {
        
        public async Task<List<PortResponse>> GetPorts()
        {
            var ports = await portMySqlRepository.GetPortsAsync();

            if (ports == null || ports.Count == 0)
                throw new NotFoundException("No ports found");

            return ports.Select(p => new PortResponse
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

        public async Task<PortRequest> GetPortById(string portId)
        {
            var port = await portMySqlRepository.GetPortByIdAsync(portId);

            if (port == null)
                throw new NotFoundException($"Port with id '{portId}' not found");

            return new PortRequest
            {
                Id = port.Id,
                Name = port.Name
            };
        }

        public async Task<PortResponse> CreatePort(PortRequest portRequest)
        {
            var port = new Port
            {
                Id = portRequest.Id,
                Name = portRequest.Name
            };

            var createdPort = await portMySqlRepository.CreatePortAsync(port);

            return new PortResponse
            {
                Id = createdPort.Id,
                Name = createdPort.Name
            };

        }

        public Task UpdatePort(string portId, PortRequest portRequest)
        {
            if (string.IsNullOrEmpty(portId))
                throw new ValidationException("Port Id cannot be null or empty");

            var port = new Port
            {
                Id = portId,
                Name = portRequest.Name
            };
            return portMySqlRepository.UpdatePortAsync(portId, port);
            
        }

        public async Task DeletePort(string portId)
        {
            if (string.IsNullOrEmpty(portId))
                throw new ValidationException("Port Id cannot be null or empty");

            await portMySqlRepository.DeletePortAsync(portId);
        }
    }

