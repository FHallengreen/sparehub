using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; set; }
}