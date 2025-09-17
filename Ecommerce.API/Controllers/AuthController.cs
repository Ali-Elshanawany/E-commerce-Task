using Ecommerce.Application.DTOs;
using Ecommerce.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("test")]

    public IActionResult test()
    {
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (result.Status != 200)
            return BadRequest(result.Message);

        return Ok(result);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        GenericResponseDto<AuthDto>? result = await _authService.Login(loginDto);

        if (result.Status != 200)
            return BadRequest(result.Message);

        return Ok(result);

    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest refreshToken)
    {
        var res = await _authService.RefreshTokenAsync(refreshToken.RefreshToken);

        if (res.Status != 200)
            return BadRequest(res.Message);

        return Ok(res);
    }



}
