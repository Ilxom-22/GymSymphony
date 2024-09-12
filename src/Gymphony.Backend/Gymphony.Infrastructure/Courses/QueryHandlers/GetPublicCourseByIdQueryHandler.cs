using AutoMapper;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Queries;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Courses.QueryHandlers;

public class GetPublicCourseByIdQueryHandler(IMapper mapper, ICourseRepository courseRepository) : IQueryHandler<GetPublicCourseByIdQuery, CourseDto>
{
    public async Task<CourseDto> Handle(GetPublicCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var foundCourse = await courseRepository.GetByIdAsync(request.CourseId)
            ?? throw new ArgumentException("Course not found!");

        return mapper.Map<CourseDto>(foundCourse);
    }
}
