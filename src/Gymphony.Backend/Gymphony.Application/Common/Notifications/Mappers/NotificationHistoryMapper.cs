using AutoMapper;
using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Notifications.Mappers;

public class NotificationHistoryMapper : Profile
{
    public NotificationHistoryMapper()
    {
        CreateMap<NotificationMessage, NotificationHistory>()
            .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.Recipient.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.ToString()))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.ToString()));
    }
}