namespace Gymphony.Domain.Entities;

public class Member : User
{
    public DateTimeOffset? BirthDay { get; set; }

    public string? StripeCustomerId { get; set; }

    public ICollection<Subscription>? Subscriptions { get; set; }
}