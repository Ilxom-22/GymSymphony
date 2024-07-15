namespace Gymphony.Application.Courses.Models.Dtos;

public class CoursesStatusGroupDto
{
    public IEnumerable<CourseDto> DraftCourses { get; set; } = default!;

    public IEnumerable<CourseDto> PublishedCourses{ get; set; } = default!;

    public IEnumerable<CourseDto> ActivatedCourses { get; set; } = default!;

    public IEnumerable<CourseDto> DeactivationRequestedCourses { get; set; } = default!;

    public IEnumerable<CourseDto> DeactivatedCourses { get; set; } = default!;
}
