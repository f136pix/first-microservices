using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        // auto-wired
        private readonly AppDbContext _context;
        
        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Platform> getAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform PlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.ID == id);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            
            _context.Platforms.Add(plat);
        }
    }
}