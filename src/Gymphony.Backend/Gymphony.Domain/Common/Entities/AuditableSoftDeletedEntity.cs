namespace Gymphony.Domain.Common.Entities;

public abstract class AuditableSoftDeletedEntity : IAuditableEntity, ISoftDeletedEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreatedTime { get; set; }
    
    public DateTimeOffset? ModifiedTime { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTimeOffset? DeletedTime { get; set; }
}