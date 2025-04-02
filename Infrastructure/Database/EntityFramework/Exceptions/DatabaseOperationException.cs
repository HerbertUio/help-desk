using Infrastructure.Database.EntityFramework.Exceptions.Common;

namespace Infrastructure.Database.EntityFramework.Exceptions;

public class DatabaseOperationException: RepositoryException
{
    public DatabaseOperationException() : base("Ocurrió un error durante la operación de base de datos.") { }

    public DatabaseOperationException(string message) : base(message) { }

    public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) { }
}