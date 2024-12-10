using System.ComponentModel.DataAnnotations;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.Address;
using Shared.Exceptions;

namespace Service.MySql.Address;

public class AddressService(IAddressRepository addressRepository) : IAddressService
{
    public async Task<List<AddressResponse>> GetAddresses()
    {
        var addresses = await addressRepository.GetAddressesAsync();
        
        return addresses.Select(a => new AddressResponse
        {
            Id = a.Id,
            AddressLine = a.AddressLine,
            PostalCode = a.PostalCode,
            Country = a.Country
        }).ToList();
    }
    public async Task<List<AddressResponse>> GetAddressBySearchQuery(string? searchQuery)
    {

        var addresses = await addressRepository.GetAddressesBySearchQueryAsync(searchQuery);
        
        return addresses.Select(a => new AddressResponse
        {
            Id = a.Id,
            AddressLine = a.AddressLine,
            PostalCode = a.PostalCode,
            Country = a.Country
        }).ToList();
    }

    public async Task<AddressResponse> GetAddressById(string addressId)
    {
        var foundAddress = await addressRepository.GetAddressByIdAsync(addressId);
        if (foundAddress == null)
        {
            throw new NotFoundException($"No warehouse found with id {addressId}");
        }
        
        return new AddressResponse
        {
            Id = foundAddress.Id,
            AddressLine = foundAddress.AddressLine,
            PostalCode = foundAddress.PostalCode,
            Country = foundAddress.Country
        };
    }

    public async Task<AddressResponse> CreateAddress(AddressRequest request)
    {
        var address = new Domain.Models.Address
        {
            AddressLine = request.AddressLine,
            PostalCode = request.PostalCode,
            Country = request.Country
        };
        
        await addressRepository.CreateAddressAsync(address);
        
        return new AddressResponse
        {
            Id = address.Id,
            AddressLine = address.AddressLine,
            PostalCode = address.PostalCode,
            Country = address.Country
        };
        
        
    }

    public async Task<AddressResponse> UpdateAddress(string addressId, AddressRequest request)
    {
        var foundAddress = await addressRepository.GetAddressByIdAsync(addressId);
        if (foundAddress == null)
        {
            throw new NotFoundException($"No address found with id {addressId}");
        }
        
        foundAddress.AddressLine = request.AddressLine;
        foundAddress.PostalCode = request.PostalCode;
        foundAddress.Country = request.Country;
        
        await addressRepository.UpdateAddressAsync(foundAddress);
        
        return new AddressResponse
        {
            Id = foundAddress.Id,
            AddressLine = foundAddress.AddressLine,
            PostalCode = foundAddress.PostalCode,
            Country = foundAddress.Country
        };
    }

    public async Task DeleteAddress(string addressId)
    {
        if (string.IsNullOrWhiteSpace(addressId))
            throw new ValidationException("Address ID cannot be null or empty.");

        await addressRepository.DeleteAddressAsync(addressId);
    }
}