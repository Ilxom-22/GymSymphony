using Gymphony.Domain.Common.Events;
using Member = Gymphony.Domain.Entities.Member;

namespace Gymphony.Application.Common.Identity.Events;

public class MemberCreatedEvent : EventBase
{
    public Member Member { get; set; } = default!;
}