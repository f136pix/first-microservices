using PlatService.DTOs;

namespace PlatService.AsyncDataServices.MessageBus
{
        public interface IMessageBusClient
        {
            void PublishNewPlatform(PlatformPublishDto platformPublishDto);
        }
}