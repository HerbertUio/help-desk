
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Extensions;
using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Responses;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterUserDto> _registerValidator;
    private readonly IValidator<LoginUserDto> _loginValidator;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository,
        IValidator<RegisterUserDto> registerValidator,
        IValidator<LoginUserDto> loginValidator,
        IConfiguration configuration)
    {
        _configuration = configuration;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _userRepository = userRepository;
    }
    public async Task<Result<UserModel>> Create(RegisterUserDto registerUserDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerUserDto);
        if (!validationResult.IsValid)
        {
            return Result<UserModel>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                "Error de validación al crear usuario."
            );
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(registerUserDto.Email);
        if (existingUser != null)
        {
            return Result<UserModel>.Failure(
               new List<string> { $"El email '{registerUserDto.Email}' ya está registrado." },
               "Conflicto al crear usuario."
           );
        }

        var user = registerUserDto.ToModel();
        user.Password = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password, workFactor: 11);
        var createdUser = await _userRepository.CreateAsync(user);
        if (createdUser == null)
        {
             return Result<UserModel>.Failure(
                 new List<string> { "No se pudo guardar el usuario en la base de datos." },
                 "Error interno al crear usuario."
             );
        }
        return Result<UserModel>.Success(createdUser, "Usuario creado con éxito.");
    }
    public async Task<Result<List<UserModel>>> GetAllAsync()
    {
        
        var userModels = await _userRepository.GetAllAsync(); 
        return Result<List<UserModel>>.Success(userModels, "Usuarios obtenidos con éxito!");
    }

    public async Task<Result<UserModel?>> GetUserByIdAsync(int id)
    {
        var userModel = await _userRepository.GetUserByIdAsync(id);
        if (userModel == null)
        {
            return Result<UserModel?>.Failure(
                new List<string> { "Usuario no encontrado." },
                "Fallo en la obtención del usuario."
            );
        }
        return Result<UserModel?>.Success(userModel, "Usuario obtenido con éxito.");
    }
    
    public async Task<Result<UserModel>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return Result<UserModel>.Failure(
                new List<string> { "Usuario no encontrado." },
                "Fallo en la actualización del usuario."
            );
        }
        updateUserDto.Id = id;
        var model = updateUserDto.ToModel();
        var updatedUser = await _userRepository.UpdateAsync(model);
        if (updatedUser == null)
        {
            return Result<UserModel>.Failure(
                new List<string> { "No se pudo actualizar el usuario." },
                "Error interno al actualizar usuario."
            );
        }
        return Result<UserModel>.Success(updatedUser, "Usuario actualizado con éxito.");
    }
    public async Task<Result<bool>> DeleteUserAsync(int id)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return Result<bool>.Failure(
                new List<string> { "Usuario no encontrado." },
                "Fallo en la eliminación del usuario."
            );
        }
        var isDeleted = await _userRepository.DeleteUserByIdAsync(id);
        if (!isDeleted)
        {
            return Result<bool>.Failure(
                new List<string> { "No se pudo eliminar el usuario." },
                "Error interno al eliminar usuario."
            );
        }
        return Result<bool>.Success(true, "Usuario eliminado con éxito.");
    }
    public async Task<Result<ResponseLoginUserDto>> Login(LoginUserDto loginUserDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginUserDto);
        if (!validationResult.IsValid)
        {
            return Result<ResponseLoginUserDto>.Failure(
                validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                "Datos de login inválidos."
            );
        }
        var user = await _userRepository.GetUserByEmailAsync(loginUserDto.Email);
        if (user == null || !user.Active)
        {
            return Result<ResponseLoginUserDto>.Failure(
                new List<string> { "Autenticación fallida." },
                "Error de autenticación."
            );
        }
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.Password);
        if (!isPasswordValid)
        {
             return Result<ResponseLoginUserDto>.Failure(
                new List<string> { "Autenticación fallida." },
                 "Error de autenticación."
            );
        }
        var token = GenerateJwtToken(user);
        var userData = user.ToDataUserDto();
        var response = new ResponseLoginUserDto
        {
            User = userData,
            Role = user.Role,
            Token = token
        };
        return Result<ResponseLoginUserDto>.Success(response, "Login exitoso.");
    }
    public async Task<Result<bool>> IsEmailUniqueAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
             return Result<bool>.Failure(new List<string>{"El email no puede estar vacío."}
                 , "Validación de email fallida.");
        }
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        bool isUnique = existingUser == null;
        return Result<bool>.Success(isUnique, isUnique ? "El email está disponible." : "El email ya está en uso.");
    }
    private string GenerateJwtToken(UserModel user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiryMinutes = Convert.ToInt32(jwtSettings["ExpiryMinutes"] ?? "60");

        if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            throw new InvalidOperationException
                ("La configuración JWT (SecretKey, Issuer, Audience) es inválida o no está completa.");
        }

        var keyBytes = Encoding.ASCII.GetBytes(secretKey);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}    
