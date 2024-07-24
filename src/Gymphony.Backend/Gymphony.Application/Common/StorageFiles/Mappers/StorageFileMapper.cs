using AutoMapper;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Gymphony.Application.Common.StorageFiles.Mappers;

public class StorageFileMapper : Profile
{
    public StorageFileMapper()
    {
        CreateMap<IFormFile, UploadCourseImageCommand>()
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.OpenReadStream()))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Length));

        CreateMap<IFormFile, UploadUserProfileImageCommand>()
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.OpenReadStream()))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Length));

        CreateMap<IFormFile, UploadStaffProfileImageCommand>()
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.OpenReadStream()))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Length));

        CreateMap<UploadCourseImageCommand, UploadFileInfoDto>()
            .ForMember(dest => dest.StorageFileType, opt => opt.MapFrom(src => StorageFileType.CourseImage));

        CreateMap<UploadProfileImageCommand, UploadFileInfoDto>()
            .ForMember(dest => dest.StorageFileType, opt => opt.MapFrom(src => StorageFileType.UserProfileImage));

        CreateMap<CourseImage, CourseImageDto>()
            .ForMember(dest => dest.CourseImageId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.StorageFile!.Url));

        CreateMap<UserProfileImage, UserProfileImageDto>()
            .ForMember(dest => dest.ProfileImageId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.StorageFile!.Url));

        CreateMap<UploadUserProfileImageCommand, UploadProfileImageCommand>();

        CreateMap<UploadStaffProfileImageCommand, UploadProfileImageCommand>();
    }
}
