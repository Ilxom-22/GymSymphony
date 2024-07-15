namespace Gymphony.Application.Courses.Models.Dtos;

public class PublicCoursesStatusGroupDto
{
    public IEnumerable<CourseDto> ActivatedCourses { get; set; } = default!;

    public IEnumerable<CourseDto> PublishedCourses { get; set; } = default!;
}
