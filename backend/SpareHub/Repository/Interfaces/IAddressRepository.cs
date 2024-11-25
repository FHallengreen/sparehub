using Domain.Models;

namespace Repository.Interfaces;

public interface IAddressRepository
{
    Task<Address> GetAddressByIdAsync(string id);
}
