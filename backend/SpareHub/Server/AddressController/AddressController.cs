using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.DTOs.Address;

namespace Server.AddressController;

[ApiController]
[Route("api/address")]
public class AddressController(IAddressService addressService) : ControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AddressResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetAddresses(string? searchQuery = null)
    {
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var addresses = await addressService.GetAddressBySearchQuery(searchQuery);
            return Ok(addresses);
        }
        var allAddresses = await addressService.GetAddresses();
        return Ok(allAddresses);
    }

    [HttpGet("{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetAddressById(string addressId)
    {
        var address = await addressService.GetAddressById(addressId);
        return Ok(address);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateAddress([FromBody] AddressRequest addressRequest) {
        
        var createdAddress = await addressService.CreateAddress(addressRequest);
        return CreatedAtAction(nameof(GetAddressById), new { addressId = createdAddress.Id }, createdAddress);
    }
    
    [HttpPut("{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateAddress(string addressId, [FromBody] AddressRequest addressRequest)
    {
        var updatedAddress = await addressService.UpdateAddress(addressId, addressRequest);
        return Ok(updatedAddress);
    }
    
    [HttpDelete("{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        await addressService.DeleteAddress(addressId);
        return Ok("Address deleted successfully");
    }
}
