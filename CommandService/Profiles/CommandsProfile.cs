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
        CreateMap<CommandReadDto, Command>();
        CreateMap<Command, CommandCreateDto>();
    }
}