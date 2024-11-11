using Microsoft.AspNetCore.Mvc;
using Service.Supplier;

namespace Server.SupplierController;

[ApiController]
[Route("api/suppliers")]
public class SupplierController(ISupplierService supplierService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetSuppliers(string? searchQuery = null)
    {
        var suppliers = await supplierService.GetSuppliersBySearchQuery(searchQuery);
        return Ok(suppliers);
    }
}
