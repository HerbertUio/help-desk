using Application.Services;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController] 
[Route("api/[controller]")] 
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseLoginUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        try
        {
            var result = await _userService.Login(loginDto);

            if (!result.IsSuccess)
            {
                _logger.LogWarning($"Intento de login fallido para {loginDto.Email}. Errores: {string.Join(", ", result.Errors)}");
                return Unauthorized();
            }

            _logger.LogInformation($"Login exitoso para {loginDto.Email}");
            return Ok(result.Data);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, $"Error inesperado en AuthController.Login para {loginDto.Email}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado procesando la solicitud.");
        }
    }
}
