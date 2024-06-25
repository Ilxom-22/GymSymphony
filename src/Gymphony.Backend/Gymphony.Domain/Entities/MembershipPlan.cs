using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public class MembershipPlan : AuditableEntity, ICreationAuditableEntity,
    IModificationAuditableEntity
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public ContentStatus Status { get; set; } = ContentStatus.Draft;

    public DateOnly? ActivationDate { get; set; }

    public DurationUnit DurationUnit { get; set; }

    public byte DurationCount { get; set; }

    public decimal Price { get; set; }

    public StripeDetails? StripeDetails { get; set; }
    
    public Guid? CreatedByUserId { get; set; }
    
    public Guid? ModifiedByUserId { get; set; }
    
    public Admin? CreatedBy { get; set; }

    public Admin? ModifiedBy { get; set; }
}