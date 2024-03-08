using CommandService.Dtos;

namespace PlatService.AsyncDataServices.MessageBus
{
        public interface IMessageBusClient
        {
            void PublishNewPlatform(PlatformPublishDto platformPublishDto);
            

            
            void PublishNewCommand(CommandPublishDto commandPublishDto);
        }
}