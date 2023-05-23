using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
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

        private static void SeedData(AppDbContext? context)
        {
            if (context is not null)
            {

                if (!context.Users.Any())
                {
                    Console.WriteLine("--> Seeding Data...");

                    context.Users.AddRange(
                        new User() { FirstName = "Chuck", LastName = "Norris", AccountCreationTime = DateTime.Now, AccountName = "chuck_norris", Email = "chuck_norris@io.com", UserRole = Role.SystemAdministrator, IsWheel = true, IsAdmin = true, IsActive = true },
                        new User() { FirstName = "Charlie", LastName = "Chaplin", AccountCreationTime = DateTime.Now, AccountName = "charlie_chaplin", Email = "charlie_chaplin@io.com", UserRole = Role.User, IsWheel = false, IsAdmin = false, IsActive = true },
                        new User() { FirstName = "Jane", LastName = "Doe", AccountCreationTime = DateTime.Now, AccountName = "jane_doe", Email = "jane_doe@io.com", UserRole = Role.User, IsWheel = false, IsAdmin = false, IsActive = false }
                    );

                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("--> We already have data");
                }
            }

        }
    }
}