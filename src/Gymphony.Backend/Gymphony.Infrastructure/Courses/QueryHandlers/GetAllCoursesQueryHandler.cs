using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Queries;
using Gymphony.Application.Products.Services;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Courses.QueryHandlers;

public class GetAllCoursesQueryHandler(ICourseRepository courseRepository, IProductsMapperService productMapperService)
    : IQueryHandler<GetAllCoursesQuery, CoursesStatusGroupDto>
{
    public Task<CoursesStatusGroupDto> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = courseRepository.Get(queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking));

        var result = productMapperService.MapToGroupedCourses(courses);

        return Task.FromResult(result);
    }
}
