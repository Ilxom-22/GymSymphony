using AutoMapper;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<SignUpDetails, Admin>();
        CreateMap<SignUpDetails, Member>();
        
        CreateMap<Member, UserDto>();
        CreateMap<Admin, UserDto>();
        CreateMap<User, UserDto>();
    }
}