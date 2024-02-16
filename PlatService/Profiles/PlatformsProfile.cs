using AutoMapper;
using PlatformService.Models;
using PlatService.DTOs;

namespace PlatService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // source  -> target
            // source : entry value 
            // target : mapped/ formated value
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto,Platform>(); // used in create platform / recieve a DTO and map it to a platform
        } 
    }
}