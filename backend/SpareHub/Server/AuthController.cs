using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.MySql.SparehubDbContext;
using Service;
using Service.MySql.Login;
using Shared.DTOs;
using Shared.DTOs.User;

namespace Server;

[ApiController]
[Route("api/auth")]
public class AuthController(JwtService jwtService, SpareHubDbContext dbContext) : ControllerBase
{


    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
    {
        var user = await dbContext.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = jwtService.GenerateToken(user.Id, user.Email, user.Role.Title);
        var loginResponse = new LoginResponse
        {
            Token = token,
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.Title
            }
        };
        return Ok(loginResponse);
    }

    [HttpGet("validate")]
    public IActionResult ValidateToken()
    {
        try
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing.");
            }

            jwtService.ValidateToken(token);

            return Ok(new { IsValid = true });
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new { IsValid = false, Error = ex.Message });
        }
    }

}