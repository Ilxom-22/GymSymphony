using AutoMapper;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Queries;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Courses.QueryHandlers;

public class GetCourseDetailsQueryHandler(ICourseRepository courseRepository, IMapper mapper) : IQueryHandler<GetCourseDetailsQuery, CourseDetailsDto>
{
    public async Task<CourseDetailsDto> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
    {
        var foundCourse = await courseRepository.Get(course => course.Id == request.CourseId,
            new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(course => course.CreatedBy)
            .Include(course => course.ModifiedBy)
            .Include(course => course.CourseImages!)
            .ThenInclude(ci => ci.StorageFile)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        return mapper.Map<CourseDetailsDto>(foundCourse);
    }
}
