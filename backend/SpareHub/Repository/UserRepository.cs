using Microsoft.EntityFrameworkCore;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;

namespace Repository;

public class UserRepository(SpareHubDbContext dbContext) : IUserRepository
{
    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
    {
        return await dbContext.Users.Include(u => u.Role).ToListAsync();
    }
}