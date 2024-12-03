using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Server.DatabaseController;

[ApiController]
[Route("/api/database")]
public class DatabaseController(IDatabaseService databaseService) : ControllerBase
{
    
    [HttpGet("tableNames")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<IEnumerable<string>>> GetTables()
    {
        var tables = await databaseService.GetTableNames();
        return Ok(tables);
    }
}
