using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class AppDbContext : DbContext
    {
        // constr
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }
        
        // mapping dbContext tables
        public DbSet<Platform> Platforms { get; set; } 
        public DbSet<Command> Commands { get; set; }  

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder
               .Entity<Platform>()
               .HasMany(p => p.Commands) // platforms has many commands
               .WithOne(p => p.Platform!) // one platforms has many
               .HasForeignKey(p => p.PlatformId); // Command.PlatformId indicates a platform
           
           modelBuilder
               .Entity<Command>()
               .HasOne(p => p.Platform) // command belongs to a single platform
               .WithMany(p => p.Commands) // one platform can have multiple commands
               .HasForeignKey(p => p.PlatformId); // Command.PlatformId indicates a platform
        }
    }
}