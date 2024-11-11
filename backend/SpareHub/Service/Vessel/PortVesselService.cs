using Neo4j.Driver;
using Shared;
using System.Linq;

namespace Service
{
    public class PortVesselService(IAsyncSession session, IVesselService vesselService) : IPortVesselService
    {
        public async Task<VesselAtPortDto> GetVesselsAtPortAsync(string portName)
        {
            var allVessels = await vesselService.GetVesselsBySearchQuery(null);

            var query = @"
            MATCH (p:Port)<-[r:DOCKS_AT]-(v)
            WHERE p.name = $portName
            RETURN p.name AS PortName, r.vessel_name AS VesselName, r.arrival_date AS ArrivalDate, r.departure_date AS DepartureDate;
            ";

            var result = await session.RunAsync(query, new { portName });

            Console.WriteLine(result);
            var vesselAtPortDto = new VesselAtPortDto
            {
                PortName = portName 
            };

            await result.ForEachAsync(record =>
            {
                string vesselName = record["VesselName"].As<string>();
                Console.WriteLine($"Neo4j returned vessel: {vesselName}");

                Console.WriteLine($"Neo4j Record: {record}");

                var matchedVessel = allVessels.FirstOrDefault(v => string.Equals(v.Name, vesselName, StringComparison.CurrentCultureIgnoreCase));
                if (matchedVessel != null)
                {
                    vesselAtPortDto.Vessels.Add(new VesselResponse
                    {
                        Id = matchedVessel.Id,
                        Name = matchedVessel.Name,
                        ImoNumber = matchedVessel.ImoNumber,
                        Flag = matchedVessel.Flag,
                        ArrivalDate = record["ArrivalDate"].As<DateTime?>(),
                        DepartureDate = record["DepartureDate"].As<DateTime?>()
                        
                    });
                }
            });

            return vesselAtPortDto;
        }
    }
}
