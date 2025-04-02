using Infrastructure.Database.EntityFramework.Exceptions.Common;

namespace Infrastructure.Database.EntityFramework.Exceptions;

public class DuplicateEntityException: RepositoryException
{
    
    public string? ConflictingValue { get; }
    public string? ConflictingField { get; }

    public DuplicateEntityException() : base("La operación violó una restricción de unicidad.") { }

    public DuplicateEntityException(string message) : base(message) { }

    public DuplicateEntityException(string message, Exception innerException) : base(message, innerException) { }

    public DuplicateEntityException(string fieldName, object fieldValue)
        : base($"Ya existe un registro con el valor '{fieldValue}' para el campo '{fieldName}'.")
    {
        ConflictingField = fieldName;
        ConflictingValue = fieldValue?.ToString();
    }
}