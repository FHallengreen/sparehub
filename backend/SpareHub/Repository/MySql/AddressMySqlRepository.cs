using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Repository.Interfaces;
using Shared;

namespace Repository.MySql;

public class AddressMySqlRepository(SpareHubDbContext dbContext, IMapper mapper) : IAddressRepository
{
    public async Task<Address> GetAddressByIdAsync(string id)
    {
        var addressEntity = await dbContext.Addresses
            .FirstOrDefaultAsync(a => a.Id == Int32.Parse(id));

        return mapper.Map<Address>(addressEntity);
    }
}