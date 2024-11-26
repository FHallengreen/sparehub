using Domain.Models;

namespace Repository.Interfaces;

public interface IAddressRepository
{
    Task<Address> GetAddressByIdAsync(string id);
    Task<Address> CreateAddressAsync(Address address);
    Task<Address> UpdateAddressAsync(Address address);
    Task DeleteAddressAsync(string id);
}
