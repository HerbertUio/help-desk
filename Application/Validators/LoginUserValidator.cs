using Domain.Dtos;
using FluentValidation;

namespace Application.Validators;

public class LoginUserValidator: AbstractValidator<LoginUserDto>
{
    public LoginUserValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El formato del email no es válido.");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.");
        
    }
}