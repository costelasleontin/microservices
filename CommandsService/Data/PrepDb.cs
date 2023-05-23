using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //use client to get all platforms
                var grpcPlatformClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcPlatformClient!.ReturnAllPlatforms();

                //use client to get all users 
                var grpcUserClient = serviceScope.ServiceProvider.GetService<IUserDataClient>();
                var users = grpcUserClient!.ReturnAllUsers();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>()!, platforms!, users!);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms, IEnumerable<User> users)
        {
            Console.WriteLine("Seeding initial platforms...");

            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformExists(plat.ExternalID))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChange();
            }

            Console.WriteLine("Seeding initial users...");

            foreach (var usr in users)
            {
                if (!repo.UserExists(usr.Id))
                {
                    repo.CreateUser(usr);
                }
                repo.SaveChange();
            }
        }
    }
}