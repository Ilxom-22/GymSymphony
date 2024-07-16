using AutoMapper;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Courses.Mappers;

public class CourseMapper : Profile
{
    public CourseMapper()
    {
        CreateMap<DraftCourseDto, Course>()
            .ForMember(dest => dest.DurationUnit, opt => opt.MapFrom(src => Enum.Parse<DurationUnit>(src.DurationUnit)));

        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DurationUnit, opt => opt.MapFrom(src => src.DurationUnit.ToString()));

        CreateMap<CreateCourseScheduleCommand, CourseSchedule>()
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => Enum.Parse<DayOfWeek>(src.Day)));

        CreateMap<(CourseSchedule Schedule, bool IsAvailable), CourseScheduleDto>()
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Schedule.Day.ToString()))
            .ForMember(dest => dest.IsAvaliable, opt => opt.MapFrom(src => src.IsAvailable))
            .IncludeMembers(src => src.Schedule);

        CreateMap<CourseSchedule, CourseScheduleDto>();

        CreateMap<Course, CourseDetailsDto>();

        CreateMap<UpdateDraftCourseCommand, DraftCourseDto>();

        CreateMap<Course, StripeProductDetails>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ProductType.Course));
    }
}
