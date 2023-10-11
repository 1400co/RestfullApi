using AutoMapper;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<PostDto, Post>().ReverseMap();
            CreateMap<SecurityDto, Security>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<PasswordRecovery, PasswordRecoveryDto>().ReverseMap();
            CreateMap<Roles, RolesDto>().ReverseMap();
            CreateMap<RolModule, RolModuleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserInRoles, UserInRolesDto>().ReverseMap();
            CreateMap<UserLogin, UserLoginDto>().ReverseMap();

        }
    }
}
