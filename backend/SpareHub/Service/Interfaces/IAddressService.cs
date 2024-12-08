using Shared.DTOs.Address;

namespace Service.Interfaces;

public interface IAddressService
{
    Task<List<AddressResponse>> GetAddresses();
    Task<List<AddressResponse>> GetAddressBySearchQuery(string? searchQuery);
    
    Task<AddressResponse> GetAddressById(string addressId);
    Task<AddressResponse> CreateAddress(AddressRequest request);
    
    Task<AddressResponse> UpdateAddress(string addressId, AddressRequest request);
    
    Task DeleteAddress(string addressId);
}