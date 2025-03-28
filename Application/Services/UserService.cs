using Application.Validators;
using Domain.Dtos;
using Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserDto> _userValidator;
    
    public UserService(IUserRepository userRepository, IValidator<UserDto> userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }
    
}