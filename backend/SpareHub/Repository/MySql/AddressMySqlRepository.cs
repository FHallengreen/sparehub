using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;
using Shared.Exceptions;

namespace Repository.MySql;

public class AddressMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IAddressRepository
{
    
    public async Task<List<Address>> GetAddressesAsync()
    {
        var addresses = await dbContext.Addresses.ToListAsync();
        return mapper.Map<List<Address>>(addresses);
    }
    public async Task<List<Address>> GetAddressesBySearchQueryAsync(string? searchQuery)
    {
        
        var addresses = await dbContext.Addresses
            .Where(a => searchQuery != null && (a.AddressLine.Contains(searchQuery) || 
                                                            a.Country.Contains(searchQuery) || 
                                                            a.PostalCode.Contains(searchQuery)))
            .ToListAsync();

        return mapper.Map<List<Address>>(addresses);
    }

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


    public async Task<Address> CreateAddressAsync(Address address)
    {
        var addressEntity = mapper.Map<AddressEntity>(address);
        await dbContext.Addresses.AddAsync(addressEntity);
        await dbContext.SaveChangesAsync();
        address.Id = addressEntity.Id.ToString();
        
        return address;
    }

    public async Task<Address> UpdateAddressAsync(Address address)
    {
        var addressEntity = mapper.Map<AddressEntity>(address);
        dbContext.Addresses.Update(addressEntity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<Address>(addressEntity);
    }

    public async Task DeleteAddressAsync(string addressId)
    {
        if (!int.TryParse(addressId, out var id))
            return;

        var addressEntity = await dbContext.Addresses
            .FirstOrDefaultAsync(d => d.Id == id);

        if (addressEntity == null)
            throw new NotFoundException("Address not found");

        dbContext.Addresses.Remove(addressEntity);
        await dbContext.SaveChangesAsync();
    }
}