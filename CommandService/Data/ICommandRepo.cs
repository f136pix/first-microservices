using CommandsService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges ();
        
        // platform 
        IEnumerable<Platform> GetAllPlatforms();
        
        bool PlatformExists(int platId);
        
        void CreatePlatform(Platform plat);
        
        // commands
        IEnumerable<Command> GetComandsFromPlatform(int platId);
        
        Command GetCommand(int platId, int comnddId);
        
        void CreateCommand(int platId, Command command);
    }
}