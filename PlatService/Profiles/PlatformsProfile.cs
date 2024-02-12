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
            // source : platform model
            // target : desired dto
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto,Platform>();
        } 
    }
}