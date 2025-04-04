using Infrastructure.Database.EntityFramework.Exceptions.Common;

namespace Infrastructure.Database.EntityFramework.Exceptions;

public class DuplicateEntityException : RepositoryException
{
    public string? ConflictingValue { get; }
    public string? ConflictingField { get; }

    public DuplicateEntityException() : base("La operación violó una restricción de unicidad.") { }

    public DuplicateEntityException(string message) : base(message) { }

    public DuplicateEntityException(string message, Exception innerException) : base(message, innerException) { }

    // Constructor útil para indicar qué campo/valor causó el conflicto
    public DuplicateEntityException(string fieldName, object fieldValue)
        : base($"Ya existe un registro con el valor '{fieldValue}' para el campo '{fieldName}'.")
    {
        ConflictingField = fieldName;
        ConflictingValue = fieldValue?.ToString();
    }
    // Sobrecarga útil para envolver la excepción original y dar un mensaje claro
    public DuplicateEntityException(string message, Exception innerException, string fieldName = null, object fieldValue = null)
        : base(message, innerException)
    {
        ConflictingField = fieldName;
        ConflictingValue = fieldValue?.ToString();
    }
}