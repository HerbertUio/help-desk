namespace Infrastructure.Database.EntityFramework.Exceptions.Common;

public class RepositoryException: Exception
{
    public RepositoryException(): base() { }
    public RepositoryException(string message) : base(message) { }
    public RepositoryException(string message, Exception innerException) : base(message, innerException) { }
}