using CommandsService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public bool PlatformExists(int platId)
        {
            return _context.Platforms.Any(p => p.Id == platId); // if any persisted Platform.Id == platId
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat)); 
            }
            _context.Platforms.Add(plat);
            
        }

        public IEnumerable<Command> GetComandsFromPlatform(int platId) =>
            _context.Commands
                .Where(c => c.PlatformId == platId)
                .OrderBy(c => c.Platform.Name);

        public Command GetCommand(int platId, int comndId)
        {
            if(platId == null || comndId == null)
            {
                throw new ArgumentNullException();
            }
            
            return _context.Commands
                .Where(c => c.PlatformId == platId && c.Id == comndId).FirstOrDefault();
        }

        public void CreateCommand(int platId, Command command)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            
            command.PlatformId = platId;
            _context.Add(command);
        }
    }
}