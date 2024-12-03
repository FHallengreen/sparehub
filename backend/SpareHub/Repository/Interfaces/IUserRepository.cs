using Persistence.MySql;

namespace Repository.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserEntity>> GetAllUsersAsync();
}