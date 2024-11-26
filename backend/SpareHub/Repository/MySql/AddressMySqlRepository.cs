using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;

namespace Repository.MySql;

public class AddressMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IAddressRepository
{
    public async Task<Address> GetAddressByIdAsync(string id)
    {
        if (!int.TryParse(id, out var parsedId))
            throw new ArgumentException("Invalid address ID format.");

        var addressEntity = await dbContext.Addresses
            .FirstOrDefaultAsync(a => a.Id == parsedId);

        if (addressEntity == null)
            throw new KeyNotFoundException($"Address with ID {id} not found.");

        return mapper.Map<Address>(addressEntity);
    }


    public Task<Address> CreateAddressAsync(Address address)
    {
        throw new NotImplementedException();
    }

    public Task<Address> UpdateAddressAsync(Address address)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAddressAsync(string id)
    {
        throw new NotImplementedException();
    }
}