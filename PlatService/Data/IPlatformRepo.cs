using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> getAllPlatforms();
        Platform PlatformById(int Id);
        void CreatePlatform(Platform plat);
    }
}
