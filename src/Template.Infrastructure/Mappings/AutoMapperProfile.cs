using AutoMapper;
using Template.Core.Dtos;
using Template.Core.DTOs;
using Template.Core.Entities;

namespace Template.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<PostDto, Post>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<RolModule, RolModuleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Modules, ModulesDto>().ReverseMap();
            CreateMap<Otp, OtpDto>().ReverseMap();
            CreateMap<Cervezas, CervezasDto>().ReverseMap();

        }
    }
}
