using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandsService.Models;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string msg)
    {
        var eventType = DetermineEvent(msg);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(msg);
                break;

            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMsg) // checks kind of event/ msg being recieved
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMsg);

        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform publish event recieved <--");
                return EventType.PlatformPublished;

            default:
                Console.WriteLine("--> Could not determine event type <--");
                return EventType.Undertemined;
        }
    }


    private void AddPlatform(string platformPublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>(); // instance of CommandRepo

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage); // json string -> platPublishedDto

            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedDto);
                if (repo.ExternalPlatformExists(plat.ExternalId))
                {
                    Console.WriteLine("--> Platform with this ExternalId already exists");
                    return;
                }
                repo.CreatePlatform(plat);
                repo.SaveChanges();
                Console.WriteLine("--> Platform was succefully saved");
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was a error adding the Platform to the DB : {e.Message}");
            }
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undertemined
}