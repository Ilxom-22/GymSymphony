using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Payments.Queries;

public class GetStripePriceIdQuery : IQuery<string>
{
    public Guid ProductId { get; set; }

    public ProductType ProductType { get; set; }
}