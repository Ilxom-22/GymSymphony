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

        CreateMap<Course, SubscriberCourseDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.CourseImages!.First()));

        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == ContentStatus.DeactivationRequested ? "In-Deactivation" : src.Status.ToString()))
            .ForMember(dest => dest.DurationUnit, opt => opt.MapFrom(src => src.DurationUnit.ToString()))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => 
            src.CourseImages != null ? src.CourseImages.First() : null));

        CreateMap<CreateCourseScheduleCommand, CourseSchedule>()
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => Enum.Parse<DayOfWeek>(src.Day)));

        CreateMap<(CourseSchedule Schedule, int Enrollments, bool IsAvailable), CourseScheduleDto>()
            .ForMember(dest => dest.IsAvaliable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.EnrollmentsCount, opt => opt.MapFrom(src => src.Enrollments))
            .IncludeMembers(src => src.Schedule);

        CreateMap<CourseSchedule, CourseScheduleDto>();

        CreateMap<Course, CourseDetailsDto>();

        CreateMap<UpdateDraftCourseCommand, DraftCourseDto>();

        CreateMap<Course, StripeProductDetails>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ProductType.Course));

        CreateMap<CourseScheduleEnrollment, MyScheduleDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseSchedule!.Course!.Name))
            .ForMember(dest => dest.Instructors, opt => opt.MapFrom(src => src.CourseSchedule!.Instructors))
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.CourseSchedule!.Day))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.CourseSchedule!.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.CourseSchedule!.EndTime));
    }
}
