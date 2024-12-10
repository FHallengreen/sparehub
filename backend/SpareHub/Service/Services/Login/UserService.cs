using AutoMapper;
using Repository.Interfaces;
using Service.Interfaces;
using Shared.DTOs.User;

namespace Service.MySql.Login;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        var user = await userRepository.GetUserByEmailAsync(email);
        return user == null ? null : mapper.Map<UserResponse>(user);
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllUsersAsync();
        return mapper.Map<IEnumerable<UserResponse>>(users);
    }
}