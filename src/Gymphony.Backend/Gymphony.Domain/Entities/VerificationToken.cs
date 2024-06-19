using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public class VerificationToken : Entity
{
    public string Token { get; set; } = default!;

    public DateTimeOffset ExpiryTime { get; set; }

    public Guid UserId { get; set; }

    public VerificationType Type { get; set; }

    public User? User { get; set; }
}