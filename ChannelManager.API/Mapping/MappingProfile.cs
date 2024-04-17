using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace ChannelManager.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserForUpdateDto>();
            CreateMap<User, UserForCreationDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserForCreationDto, User>();
            CreateMap<UserDto, User>();

            CreateMap<Post, PostDto>();
            CreateMap<Post, PostForUpdateDto>();
            CreateMap<Post, PostForCreationDto>();
            CreateMap<PostDto, Post>();
            CreateMap<PostForUpdateDto, Post>();
            CreateMap<PostForCreationDto, Post>();
        }
    }
}
