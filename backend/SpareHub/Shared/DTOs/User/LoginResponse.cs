using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User;

public class LoginResponse
{
    public required string Token { get; init; }
    public required UserResponse User { get; init; }
}