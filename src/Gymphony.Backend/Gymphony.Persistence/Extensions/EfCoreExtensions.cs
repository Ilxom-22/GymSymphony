using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Common.Queries;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Extensions;

public static class EfCoreExtensions
{
    private static IQueryable<TSource> ApplyTrackingMode<TSource>(
        this IQueryable<TSource> source,
        QueryTrackingMode trackingMode) where TSource : class, IEntity
    {
        return trackingMode switch
        {
            QueryTrackingMode.AsTracking => source,
            QueryTrackingMode.AsNoTracking => source.AsNoTracking(),
            QueryTrackingMode.AsNoTrackingWithIdentityResolution => source.AsNoTrackingWithIdentityResolution(),
            _ => source
        };
    }

    public static IQueryable<TSource> ApplyQueryOptions<TSource>(
        this IQueryable<TSource> source,
        QueryOptions queryOptions) where TSource : class, IEntity
    {
        return source.ApplyTrackingMode(queryOptions.TrackingMode);
    }
}