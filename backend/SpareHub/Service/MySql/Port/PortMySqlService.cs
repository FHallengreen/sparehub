
using Repository.MySql;
using Service.Interfaces;
using Shared.DTOs.Port;
using Shared.Exceptions;


namespace Service.MySql.Port;

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

        public async Task<PortResponse> GetPortById(string portId)
        {
            var port = await portMySqlRepository.GetPortByIdAsync(portId);

            if (port == null)
                throw new NotFoundException($"Port with id '{portId}' not found");

            return new PortResponse()
            {
                Id = port.Id,
                Name = port.Name
            };
        }

        public async Task<PortResponse> CreatePort(PortRequest portRequest)
        {
            var port = new Domain.Models.Port
            {
                Name = portRequest.Name
            };

            var createdPort = await portMySqlRepository.CreatePortAsync(port);

            return new PortResponse
            {
                Id = createdPort.Id,
                Name = createdPort.Name
            };

        }

        public async Task<PortResponse> UpdatePort(string portId, PortRequest portRequest)
        {
            var port = await portMySqlRepository.GetPortByIdAsync(portId);
            if (port == null)
                throw new NotFoundException($"Port with id '{portId}' not found");

            // Update properties
            port.Name = portRequest.Name;

            // Save changes through the repository
             await portMySqlRepository.UpdatePortAsync(portId, port);

            // Prepare the response
            return new PortResponse
            {
                Id = port.Id,
                Name = port.Name
            };
        }


        public async Task DeletePort(string portId)
        {
            var port = await portMySqlRepository.GetPortByIdAsync(portId);
            if (port == null)
                throw new NotFoundException($"Port with id '{portId}' not found");

            await portMySqlRepository.DeletePortAsync(portId);
        }
    }

