using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripePriceRecurringDto
{
    public string Interval { get; set; } = DurationUnit.Month.ToLowerString();

    public int IntervalCount { get; set; } = 1;
}