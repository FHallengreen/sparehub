using Shared.DTOs.User;

namespace Service.Interfaces;

public interface IUserService
{
    Task<UserResponse?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
}