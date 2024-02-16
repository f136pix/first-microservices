using PlatService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        // Task type represents a async operation
        Task sendPlatformToCommand(PlatformReadDto plat);
    } 
}

