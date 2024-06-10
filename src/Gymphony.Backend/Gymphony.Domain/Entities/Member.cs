namespace Gymphony.Domain.Entities;

public class Member : User
{
    public DateTimeOffset? BirthDay { get; set; }
}