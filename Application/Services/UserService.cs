
using System.Security.Cryptography;
using Application.Extensions;
using Application.Validators;
using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Responses;
using FluentValidation;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserModel> _validator;
    private readonly IValidator<RegisterUserDto> _registerValidator;
    
    public UserService(IUserRepository userRepository, IValidator<UserModel> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }
    public async Task<Result<List<UserDto>>> GetAllUsers()
    {
        var result = await _userRepository.GetAllAsync();
        return Result<List<UserDto>>.Success(result, "Usuarios obtenidos con exito!");
    }
    public async Task<Result<List<UserDto>>> GetUserById(int id)
    {
        var result = await _userRepository.GetUserByIdAsync(id);
        if (result == null)
        {
            return Result<List<UserDto>>.Failure(new List<string> { "Usuario no encontrado." },
                "Fallo en la obtención del usuario.");
        }
        return Result<List<UserDto>>.Success(result, "Usuario obtenido con éxito.");
    }
    public async Task<Result<UserModel>> Create(RegisterUserDto registerUserDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerUserDto);
        var passwordEncrypt = getmd5(registerUserDto.Password);
        registerUserDto.Password = passwordEncrypt;
        if (!validationResult.IsValid)
        {
            return Result<UserModel>
                .Failure(validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList(), "Error de validación");
        }
        var user = registerUserDto.ToModel();
        var result = await _userRepository.CreateAsync(user);
        return Result<UserModel>.Success(result, "Usuario creado con exito.");
    }
    public async Task<ResponseLoginUserDto> Login(LoginUserDto loginUserDto)
    {
        return await _userRepository.LoginAsync(loginUserDto);
    }
    
    
    
    
    public async Task<bool> IsUniqueUser(string user)
    {
        if (string.IsNullOrEmpty(user))
        {
            return false; //TODO: Implementar una excepcion
        }
        var existingUser = await _userRepository.GetUserByName(user);
        if (existingUser == null)
        {
            return true;
        }
        return false;
    } 
    public static string getmd5(string password)
    {
        MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
        data = x.ComputeHash(data);
        string resp = "";
        for (int i = 0; i < data.Length; i++)
        {
            resp += data[i].ToString("x2").ToLower();
        }
        return resp;
    }
    
}