using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Domain.Dtos;
using FluentValidation;

namespace Application.Validators;

public class UserValidator: AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(n => n.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacio.")
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("El nombre contiene caracteres inválidos.")
            .When(n => !string.IsNullOrWhiteSpace(n.Name))
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(50).WithMessage("El nombre debe tener menos de 50 caracteres.");
        
        RuleFor(l => l.LastName)
            .NotEmpty().WithMessage("El apellido no puede estar vacio. ")
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("El apellido contiene caracteres inválidos.")
            .When(l => !string.IsNullOrWhiteSpace(l.LastName))
            .MinimumLength(2).WithMessage("El apellido debe tener al menos 2 caracteres.")
            .MaximumLength(50).WithMessage("El apellido debe tener menos de 50 caracteres.");


        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .Must(BeAValidEmail).WithMessage("El formato del email no es válido.");
        
    }

    private bool BeAValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string emailPattern = @"^(?!.*\.\.)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
}