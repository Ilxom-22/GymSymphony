namespace Gymphony.Domain.Common.Queries;

public struct QueryOptions(QueryTrackingMode trackingMode)
{
    public QueryTrackingMode TrackingMode { get; set; } = trackingMode;
}