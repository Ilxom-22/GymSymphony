using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public abstract class Product : AuditableEntity, ICreationAuditableEntity, IModificationAuditableEntity
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public ContentStatus Status { get; set; } = ContentStatus.Draft;
    
    public DurationUnit DurationUnit { get; set; }

    public int DurationCount { get; set; }
    
    public decimal Price { get; set; }

    public ProductType Type { get; set; }

    public DateOnly? ActivationDate { get; set; }

    public DateOnly? DeactivationDate { get; set; }

    public StripeDetails? StripeDetails { get; set; }
    
    public Guid? CreatedByUserId { get; set; }
    
    public Guid? ModifiedByUserId { get; set; }
    
    public Admin? CreatedBy { get; set; }

    public Admin? ModifiedBy { get; set; }
}