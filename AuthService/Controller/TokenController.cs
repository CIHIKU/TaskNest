using AuthService.Models.Requests;
using AuthService.Services;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controller;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        // Delegate to the token service to handle the business logic
        var response = await _tokenService.RefreshTokenAsync(request);
        return Ok(response);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        await _tokenService.DeleteRefreshTokenAsync(request.RefreshToken, ipAddress);
        // No need to invalidate the JWT as it will expire on its own
        return Ok(new { Message = "You have logged out successfully" });
    }
}