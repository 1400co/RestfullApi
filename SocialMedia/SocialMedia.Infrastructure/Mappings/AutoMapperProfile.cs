using AutoMapper;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<PostDto, Post>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Roles, RolesDto>().ReverseMap();
            CreateMap<RolModule, RolModuleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserInRoles, UserInRolesDto>().ReverseMap();
            CreateMap<Modules, ModulesDto>().ReverseMap();
            CreateMap<Otp, OtpDto>().ReverseMap();

        }
    }
}
