using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Server.UserController;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{email}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmailAsync(email);
        if (user == null) return NotFound("User not found.");
        return Ok(user);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }
}