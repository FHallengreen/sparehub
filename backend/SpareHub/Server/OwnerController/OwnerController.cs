using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces;
using Shared.DTOs.Owner;
using Shared.Exceptions;

namespace Server.OwnerController;

[ApiController]
[Route("api/owner")]
[Authorize]
public class OwnerController(IOwnerService ownerService) : ControllerBase
{
    
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetOwners()
    {
        var owners = await ownerService.GetOwners();
        return Ok(owners);
    }
    
    [HttpGet ("{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetOwnerById(string ownerId)
    {
        var owner = await ownerService.GetOwnerById(ownerId);
        return Ok(owner);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OwnerResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreateOwner(OwnerRequest ownerRequest)
    {
        var owner = await ownerService.CreateOwner(ownerRequest);
        return CreatedAtAction(nameof(GetOwnerById), new { ownerId = owner.Id }, owner);
    }
    
    [HttpPut ("{ownerId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OwnerResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> UpdateOwner(string ownerId, [FromBody]OwnerRequest ownerRequest)
    {
        var owner = await ownerService.UpdateOwner(ownerId, ownerRequest);
        return Ok(owner);
    }
    
    [HttpDelete ("{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteOwner(string ownerId)
    {
        await ownerService.DeleteOwner(ownerId);
        return Ok("Owner deleted");
    }
}