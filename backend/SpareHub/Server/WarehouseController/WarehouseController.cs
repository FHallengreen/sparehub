using Microsoft.AspNetCore.Mvc;
using Service.Warehouse;

namespace Server.WarehouseController;

[ApiController]
[Route("api/warehouse")]
public class WarehouseController(IWarehouseService warehouseService) : ControllerBase
{
    
    [HttpGet("search")]
    public async Task<IActionResult> GetWarehouses(string? searchQuery = null)
    {
        try
        {
            var warehouses = await warehouseService.GetWarehousesBySearchQuery(searchQuery);

            if (!warehouses.Any())
            {
                return NotFound("No warehouses found matching the search criteria.");
            }
            return Ok(warehouses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
}
