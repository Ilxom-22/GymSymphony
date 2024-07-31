using AutoMapper;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Application.Products.Services;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Products.Services;

public class ProductsMapperService(IMapper mapper) : IProductsMapperService
{
    public MembershipPlansStatusGroupDto MapToGroupedPlans(IEnumerable<MembershipPlan> plans)
    {
        var groupedPlans = GroupPlansByStatus(plans);

        var result = new MembershipPlansStatusGroupDto
        {
            DraftPlans = MapMembershipPlans(groupedPlans, ContentStatus.Draft),
            PublishedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Published),
            ActivatedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Activated),
            DeactivationRequestedPlans = MapMembershipPlans(groupedPlans, ContentStatus.DeactivationRequested),
            DeactivatedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Deactivated),
        };

        return result;
    }

    public PublicMembershipPlansStatusDto MapToPublicGroupedPlans(
        IEnumerable<MembershipPlan> plans)
    {
        var groupedPlans = GroupPlansByStatus(plans);

        var result = new PublicMembershipPlansStatusDto
        {
            ActivatedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Activated),
            PublishedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Published)
        };

        return result;
    }

    public CoursesStatusGroupDto MapToGroupedCourses(IEnumerable<Course> courses)
    {
        var groupedCouses = GroupCoursesByStatus(courses);

        var result = new CoursesStatusGroupDto
        {
            DraftCourses = MapCourses(groupedCouses, ContentStatus.Draft),
            PublishedCourses = MapCourses(groupedCouses, ContentStatus.Published),
            ActivatedCourses = MapCourses(groupedCouses, ContentStatus.Activated),
            DeactivationRequestedCourses = MapCourses(groupedCouses, ContentStatus.DeactivationRequested),
            DeactivatedCourses = MapCourses(groupedCouses, ContentStatus.Deactivated),
        };

        return result;
    }

    public PublicCoursesStatusGroupDto MapToPublicGroupedCourses(IEnumerable<Course> courses)
    {
        var groupedCouses = GroupCoursesByStatus(courses);

        var result = new PublicCoursesStatusGroupDto
        {
            ActivatedCourses = MapCourses(groupedCouses, ContentStatus.Activated),
            PublishedCourses = MapCourses(groupedCouses, ContentStatus.Published)
        };

        return result;
    }

    private Dictionary<ContentStatus, List<MembershipPlan>> GroupPlansByStatus(IEnumerable<MembershipPlan> plans) =>
         plans
            .GroupBy(p => p.Status)
            .ToDictionary(g => g.Key, g => g.ToList());


    private IEnumerable<MembershipPlanDto> MapMembershipPlans(Dictionary<ContentStatus, List<MembershipPlan>> groupedPlans, ContentStatus status) =>
         groupedPlans.TryGetValue(status, out var plan)
            ? mapper.Map<IEnumerable<MembershipPlanDto>>(plan)
            : [];


    private Dictionary<ContentStatus, List<Course>> GroupCoursesByStatus(IEnumerable<Course> courses) =>
        courses
            .GroupBy(c => c.Status)
            .ToDictionary(g => g.Key, g => g.ToList());

    private IEnumerable<CourseDto> MapCourses(Dictionary<ContentStatus, List<Course>> groupedCourses, ContentStatus status) =>
        groupedCourses.TryGetValue(status, out var course)
            ? mapper.Map<IEnumerable<CourseDto>>(course)
            : [];
}