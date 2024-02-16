using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> APP V2 <--");
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