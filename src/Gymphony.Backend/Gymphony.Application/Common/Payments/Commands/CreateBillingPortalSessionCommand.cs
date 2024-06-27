using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Payments.Commands;

public class CreateBillingPortalSessionCommand : ICommand<string>
{
    public string ReturnUrl { get; set; } = default!;
}