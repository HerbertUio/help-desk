using Infrastructure.Database.EntityFramework.Exceptions.Common;

namespace Infrastructure.Database.EntityFramework.Exceptions;

public class EntityNotFoundException: RepositoryException
{
    public object? EntityId { get; }
    public Type? EntityType { get; }
    
    public EntityNotFoundException() : base("La entidad solicitada no fue encontrada.") { }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    
    public EntityNotFoundException(Type entityType, object entityId)
        : base($"La entidad de tipo '{entityType.Name}' con Id '{entityId}' no fue encontrada.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public EntityNotFoundException(string entityName, object entityId)
        : base($"La entidad '{entityName}' con Id '{entityId}' no fue encontrada.")
    {
        EntityId = entityId;
    }
}