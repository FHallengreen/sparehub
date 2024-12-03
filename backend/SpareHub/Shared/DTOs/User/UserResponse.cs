namespace Shared.DTOs.User;

public class UserResponse
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public string? Role { get; init; }
}