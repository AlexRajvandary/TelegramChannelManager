﻿using AutoMapper;
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

            CreateMap<Post, PostDto>();
            CreateMap<Post, PostForUpdateDto>();
            CreateMap<Post, PostForCreationDto>();
        }
    }
}
