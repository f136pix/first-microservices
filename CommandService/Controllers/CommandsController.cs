using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/platforms/{platId}/[controller]")] // api/c/platforms/1/commands
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpGet]                                                             // url param 
    public ActionResult<IEnumerable<CommandReadDto>> GetComandsFromPlatform(int platId)
    {
        Console.WriteLine($"--> Retrieving commands from platform with Id {platId}");
        
        if(!_repository.PlatformExists(platId))
        {
            return NotFound();
        }
        
        var commands = _repository.GetComandsFromPlatform(platId);
        var mappedComands = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
        
        return Ok(mappedComands);
        
    }
    
    [HttpGet("{commandId}", Name = "GetCommandByPlatformAndCommandId")]
    public ActionResult<CommandReadDto> GetCommandByPlatformAndCommandId(int platId, int commandId) // both attr are url params
    {
        Console.WriteLine($"--> Getting command for platform PlatId {platId} / CommandId {commandId}");
        
        if(!_repository.PlatformExists(platId))
        {
            return NotFound();
        }
        
        var retrievedCommand = _repository.GetCommand(platId, commandId);
        
        if(retrievedCommand == null )
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<CommandReadDto>(retrievedCommand));
    }
    
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platId, CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> Creating a new command for Platform {platId}");
        Console.WriteLine($"--> Command : {commandCreateDto.HowTo}");
        
        if(!_repository.PlatformExists(platId))
        {
            return NotFound();
        }
        
        var command = _mapper.Map<Command>(commandCreateDto);
        
        _repository.CreateCommand(platId, command);
        _repository.SaveChanges();
        
        var commandReadDto = _mapper.Map<CommandReadDto>(command);
        
        return CreatedAtRoute(nameof(GetCommandByPlatformAndCommandId),
            new {platId = platId, commandId = commandReadDto.Id},
            commandReadDto);
    }
}