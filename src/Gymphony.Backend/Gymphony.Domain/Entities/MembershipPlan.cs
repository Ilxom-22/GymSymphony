using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Domain.Structs;

namespace Gymphony.Domain.Entities;

public class MembershipPlan : AuditableEntity, ICreationAuditableEntity,
    IModificationAuditableEntity
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public ContentStatus Status { get; set; } = ContentStatus.Draft;

    public DateOnly? ActivationDate { get; set; }

    public decimal Price { get; set; }
    
    public Guid? CreatedByUserId { get; set; }
    
    public Guid? ModifiedByUserId { get; set; }
    
    public Admin? CreatedBy { get; set; }

    public Admin? ModifiedBy { get; set; }

    public Duration Duration
    {
        get => new Duration { Days = _days, Months = _months };
        set
        {
            _days = value.Days;
            _months = value.Months;
        }
    }

    private byte _days;
    private byte _months;
}