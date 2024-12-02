using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.MySql.SparehubDbContext;
using Service;
using Service.MySql.Login;
using Shared.DTOs;
using Shared.DTOs.User;

namespace Server;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtService jwtService, SpareHubDbContext dbContext) : ControllerBase
{

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
    {
        var user = await dbContext.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        // Pass email and role to GenerateToken
        var token = jwtService.GenerateToken(user.Id, user.Email, user.Role.Title);
        return Ok(new { Token = token });
    }



}