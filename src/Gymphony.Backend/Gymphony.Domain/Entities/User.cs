using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public abstract class User : AuditableEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public Role Role { get; set; }

    public string EmailAddress { get; set; } = default!;

    public AccountStatus Status { get; set; }

    public Provider AuthenticationProvider { get; set; }

    public string AuthDataHash { get; set; } = default!;

    public UserProfileImage? ProfileImage { get; set; }

    public AccessToken? AccessToken { get; set; }
    
    public RefreshToken? RefreshToken { get; set; }

    public VerificationToken? VerificationToken { get; set; }
}