using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Identity.Events;

public class ForgotPasswordEvent : EventBase
{
    public string EmailAddress { get; set; } = default!;
}