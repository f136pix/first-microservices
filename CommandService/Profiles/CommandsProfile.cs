using AutoMapper;
using CommandService.Dtos;
using CommandsService.Models;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // source  -> target
        // source : entry value 
        // target : mapped/ formated value
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<CommandReadDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<Command, CommandPublishDto>();
        CreateMap<Command, CommandCreateDto>();
        CreateMap<PlatformPublishDto, Platform>() // Platform.ExternalId = PlatformPublishedDto.Id
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom((src => src.Id)));
    }
}