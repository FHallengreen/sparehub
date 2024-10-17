using Neo4j.Driver;
using Shared;
using System.Linq;

namespace Service
{
    public class PortVesselService(IAsyncSession session, IVesselService vesselService) : IPortVesselService
    {
        public async Task<VesselAtPortDto> GetVesselsAtPortAsync(string portName)
        {
            // Step 1: Fetch all vessels from MySQL
            var allVessels = await vesselService.GetAllVessels();

            // Step 2: Query Neo4j for vessels docking at the specified port
            var query = @"
            MATCH (p:Port)<-[r:DOCKS_AT]-(v)
            WHERE p.name = $portName
            RETURN p.name AS PortName, r.vessel_name AS VesselName, r.arrival_date AS ArrivalDate, r.departure_date AS DepartureDate;
            ";

            var result = await session.RunAsync(query, new { portName });

            Console.WriteLine(result);
            var vesselAtPortDto = new VesselAtPortDto
            {
                PortName = portName // Set the port name
            };

            // Step 3: Log what Neo4j returns before filtering
            await result.ForEachAsync(record =>
            {
                // Log the vessel name from Neo4j
                string vesselName = record["VesselName"].As<string>();
                Console.WriteLine($"Neo4j returned vessel: {vesselName}");

                // Log the full record for debugging
                Console.WriteLine($"Neo4j Record: {record}");

                // Find the corresponding vessel in MySQL
                var matchedVessel = allVessels.FirstOrDefault(v => v.Name.ToLower() == vesselName.ToLower());
                if (matchedVessel != null)
                {
                    // Add the vessel details along with the docking information from Neo4j
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
