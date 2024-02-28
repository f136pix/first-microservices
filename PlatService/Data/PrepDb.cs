using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isDev)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isDev);
            }
        }

        private static void SeedData(AppDbContext context, bool isDev)
        {
            Console.WriteLine("--> App V10");
            if (!isDev)
            {
                Console.WriteLine("--> Making Migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"-->  Error in migrations :{e}");
                }
            }
            
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data <--");
                context.Platforms.AddRange(
                    new Platform() { Name = "DotNET", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "TypeScript", Publisher = "Microsoft", Cost = "Free" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Data already exists in the context <--");
            }
        }
    }
}

