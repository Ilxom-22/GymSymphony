using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Queries;
using Gymphony.Application.Products.Services;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Courses.QueryHandlers;

public class GetPublicCoursesQueryHandler(ICourseRepository courseRepository, IProductsMapperService productsMapperService)
    : IQueryHandler<GetPublicCoursesQuery, PublicCoursesStatusGroupDto>
{
    public Task<PublicCoursesStatusGroupDto> Handle(GetPublicCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = courseRepository.Get(course => course.Status == ContentStatus.Activated 
        || course.Status == ContentStatus.Published, queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking));

        var result = productsMapperService.MapToPublicGroupedCourses(courses);

        return Task.FromResult(result);
    }
}
