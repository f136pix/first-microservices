using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using PlatService.AsyncDataServices.MessageBus;
using PlatService.DTOs;

namespace PlatformService.Controllers
{
    // extends default api controller
    [Route("api/[controller]")] // api/Platforms
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        // autowired
        public PlatformsController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient
        )
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Retrieving Platforms");
            var platformItems = _repository.getAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.PlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();
            // transform PlatformModel in a PlatformReadDto
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            // send sync http message
            try
            {
                await _commandDataClient.sendPlatformToCommand(platformReadDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Error sending HTTP synchronous req : {e.Message}");
            }

            // send async rabbitmq message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published"; // kind of event being thrown in the bus
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception e)
            {

                Console.WriteLine($"--> Error sending asyncronous req : {e.Message}");
            }

            Console.WriteLine(platformReadDto);
            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.id },
                platformReadDto); // returning the created platform navigation in headers
        }
    }
}