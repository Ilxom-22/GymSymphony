namespace Gymphony.Domain.Enums;

public enum DurationUnit
{
    Day = 0,
    Week = 1,
    Month = 2,
    Year = 3
}

public static class DurationUnitExtensions
{
    public static string ToLowerString(this DurationUnit durationUnit) =>
        durationUnit.ToString().ToLower();
}