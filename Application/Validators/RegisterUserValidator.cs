using Domain.Dtos;
using FluentValidation;

namespace Application.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(n => n.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacío.")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
            .MaximumLength(50).WithMessage("El nombre debe tener menos de 50 caracteres.")
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("El nombre contiene caracteres inválidos.")
                .When(n => !string.IsNullOrWhiteSpace(n.Name));

        RuleFor(l => l.LastName)
            .NotEmpty().WithMessage("El apellido no puede estar vacío.")
            .MinimumLength(3).WithMessage("El apellido debe tener al menos 3 caracteres.")
            .MaximumLength(50).WithMessage("El apellido debe tener menos de 50 caracteres.")
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("El apellido contiene caracteres inválidos.")
                .When(l => !string.IsNullOrWhiteSpace(l.LastName));

        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El formato del email no es válido.");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .MaximumLength(16).WithMessage("La contraseña no puede tener más de 16 caracteres.")
            .Matches(@"[A-Z]+").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches(@"[a-z]+").WithMessage("La contraseña debe contener al menos una letra minúscula.")
            .Matches(@"[0-9]+").WithMessage("La contraseña debe contener al menos un número.")
            .Matches(@"[\!\?\*\.\-_@#\$%^&+=]").WithMessage("La contraseña debe contener al menos un carácter especial (!?*.-_@#$%^&+=)."); // Ampliado set de especiales

        RuleFor(p => p.PhoneNumber)
             .NotEmpty().WithMessage("El número de teléfono es requerido.")
             .Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$").WithMessage("El formato del número de teléfono no es válido.") // Regex básica para teléfono
                 .When(p => !string.IsNullOrWhiteSpace(p.PhoneNumber));

        RuleFor(d => d.DepartmentId)
            .GreaterThan(0).WithMessage("El ID de departamento no es válido.");

        RuleFor(r => r.Role)
            .NotEmpty().WithMessage("El rol es requerido.");
    }
}