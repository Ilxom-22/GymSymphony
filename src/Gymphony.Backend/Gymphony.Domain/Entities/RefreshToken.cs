using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class RefreshToken : IEntity
{
    Guid IEntity.Id
    {
        get => UserId;
        set => UserId = value;
    }

    public string Token { get; set; } = default!;

    public DateTimeOffset ExpiryTime { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}