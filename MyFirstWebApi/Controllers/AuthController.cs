using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.DTOs;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var token = await authService.Login(
            loginDto.Email,
            loginDto.Password);

        if (token == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(new
        {
            token
        });
    }
}