using System.Text;
using AutoMapper;
using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Notifications.Mappers;

public class NotificationTemplateMapper : Profile
{
    public NotificationTemplateMapper()
    {
        CreateMap<NotificationTemplate, NotificationMessage>()
            .ForMember(dest => dest.TemplateId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => new StringBuilder(src.Title)))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => new StringBuilder(src.Content)));
    }
}