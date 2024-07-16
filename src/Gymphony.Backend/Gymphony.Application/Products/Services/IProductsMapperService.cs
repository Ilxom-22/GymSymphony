using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Products.Services;

public interface IProductsMapperService
{
    MembershipPlansStatusGroupDto MapToGroupedPlans(IEnumerable<MembershipPlan> plans);

    PublicMembershipPlansStatusDto MapToPublicGroupedPlans(IEnumerable<MembershipPlan> plans);

    CoursesStatusGroupDto MapToGroupedCourses(IEnumerable<Course> courses);

    PublicCoursesStatusGroupDto MapToPublicGroupedCourses(IEnumerable<Course> courses);
}