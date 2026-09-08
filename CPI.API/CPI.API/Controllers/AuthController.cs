using CPI.API.CPI.Application.DTOs;
using CPI.API.CPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPI.API.CPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ILoginService _loginService;
    private readonly IUserAuthService _userAuthService;

    public AuthController(ILogger<AuthController> logger, ILoginService loginService, IUserAuthService userAuthService)
    {
        _logger = logger;
        _loginService = loginService;
        _userAuthService = userAuthService;
    }

    private static string SanitizeForLog(string? value)
    {
        return (value ?? string.Empty)
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty);
    }

    [HttpPost("signin")]
    [AllowAnonymous]
    public IActionResult Signin([FromBody] UserDTO user)
    {
        var sanitizedUserName = SanitizeForLog(user.UserName);
        _logger.LogInformation("Attempting to sign in user: {UserName}", sanitizedUserName);
        
        if(user == null || string.IsNullOrEmpty(user.UserName))
        {
            _logger.LogWarning("Signin attempt failed: User object is null or UserName is empty.");
            return BadRequest("User object is null or UserName is empty.");
        }

        var token = _loginService.ValidateCredentials(user);
        if (token == null) return Unauthorized();

        _logger.LogInformation("User signed in successfully: {UserName}", sanitizedUserName);

        return Ok(token);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public IActionResult Refresh([FromBody] TokenDTO token)
    {
        if(token == null) return BadRequest("Token object is null.");

        var newToken = _loginService.ValidateCredentials(token);
        if (newToken == null) return Unauthorized();
        
        return Ok(newToken);
    }

    [HttpPost("revoke")]
    [AllowAnonymous]
    public IActionResult Revoke()
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(userName)) return BadRequest("Invalid user context!");

        var result = _loginService.RevokeToken(userName);
        if (!result) return BadRequest("Invalid user context!");
        return Ok();
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public IActionResult Create([FromBody] AccountCredentialsDTO user)
    {
        if (user == null) return BadRequest("User object is null.");

        var createdUser = _loginService.Create(user);
        if (createdUser == null) return BadRequest("Failed to create user.");

        return Ok();
    }
}
