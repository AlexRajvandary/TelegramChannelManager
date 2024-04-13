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
        }
    }
}
