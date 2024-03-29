using System.Text;
using System.Text.Json;
using PlatService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        
        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task sendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json");

            Console.WriteLine($"{_configuration["CommandService"]}");
            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}",httpContent);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Task method response OK");
            }
            else
            {
                Console.WriteLine("--> Sync Task method response BAD");
            }
        }
    }
}