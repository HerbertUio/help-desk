using Application.Extensions;
using Application.Services;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(UserService userService, ILogger<UsersController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation($"Solicitud para obtener todos los usuarios recibida.");
        try
        {
            var result = await _userService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError($"Error del servicio al obtener todos los usuarios: {result.Message} - {result.Errors}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error al obtener usuarios.", Details = result.Errors });
            }
            
            var userDtos = result.Data.Select(u => u.ToUserDto()).ToList();
            _logger.LogInformation($"Devolviendo {userDtos.Count} usuarios.");
            return Ok(userDtos); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Excepción no controlada en GetAllUsers.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado en el servidor.");
        }
    }
    
    [HttpGet("{id:int}", Name = "GetUserById")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(int id)
    {
        _logger.LogInformation($"Solicitud para obtener usuario por ID: {id}");
        try
        {
            var result = await _userService.GetUserByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning($"Usuario no encontrado con ID: {id}");
                return NotFound(new { Message = $"Usuario con ID {id} no encontrado." }); 
            }
            var userDto = result.Data.ToUserDto();
            _logger.LogInformation($"Usuario encontrado con ID: {id}");
            return Ok(userDto); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Excepción no controlada en GetUserById para ID: {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado en el servidor.");
        }
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserDto registerDto)
    {
        _logger.LogInformation($"Solicitud para crear usuario recibida para Email: {registerDto.Email}");
        try
        {
            var result = await _userService.Create(registerDto);

            if (!result.IsSuccess)
            {
                _logger.LogWarning($"Fallo al crear usuario para Email: {registerDto.Email}. Errores: {string.Join("; ", result.Errors)}");
                return ValidationProblem(title: result.Message, detail: string.Join("; ", result.Errors));
            }
            var createdUserDto = result.Data.ToUserDto();
            _logger.LogInformation($"Usuario creado con éxito con ID: {createdUserDto.Id}");
            return CreatedAtAction(nameof(GetUserById), new { id = createdUserDto.Id }, createdUserDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Excepción no controlada en CreateUser para Email: {registerDto.Email}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear usuario.");
        }
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
    {
        _logger.LogInformation($"Solicitud para actualizar usuario ID: {id}");
        try
        {
            var result = await _userService.UpdateUserAsync(id, updateDto);

            if (!result.IsSuccess)
            {
                _logger.LogWarning($"Fallo al actualizar usuario ID: {id}. Errores: {string.Join("; ", result.Errors)}");
                return ValidationProblem(title: result.Message, detail: string.Join("; ", result.Errors));
            }
            var updatedUserDto = result.Data.ToUserDto();
            _logger.LogInformation($"Usuario actualizado con éxito ID: {id}");
            return Ok(updatedUserDto); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Excepción no controlada en UpdateUser para ID: {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar usuario.");
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation($"Solicitud para eliminar usuario ID: {id}");
        try
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result.IsSuccess || !result.Data)
            {
                _logger.LogWarning($"No se encontró o no se pudo eliminar usuario ID: {id}. Errores: {string.Join("; ", result.Errors)}");
                return NotFound(new { Message = $"Usuario con ID {id} no encontrado o no pudo ser eliminado." }); 
            }
            _logger.LogInformation($"Usuario eliminado con éxito ID: {id}");
            return NoContent();
        }
        catch (Infrastructure.Database.EntityFramework.Exceptions.EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, $"EntityNotFoundException en DeleteUser para ID: {id}");
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Excepción no controlada en DeleteUser para ID: {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar usuario.");
        }
    }
}