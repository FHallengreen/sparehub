using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Server.SupplierController;

[ApiController]
[Route("api/supplier")]
[Authorize]
public class SupplierController(ISupplierService supplierService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetSuppliers(string? searchQuery = null)
    {
        var suppliers = await supplierService.GetSuppliersBySearchQuery(searchQuery);
        return Ok(suppliers);
    }
}
