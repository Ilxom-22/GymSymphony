using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class Admin : User, ICreationAuditableEntity
{
    public Guid CreatedByUserId { get; set; }

    public bool TemporaryPasswordChanged { get; set; }
}