using CommandsService.Models;

namespace CommandsService.Data{
    public interface ICommandRepo{
        bool SaveChange();

        //Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        IEnumerable<Command> GetCommandsForUserAndPlatform(int userId,int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);

        //Users
        IEnumerable<User> GetAllUsers();
        void CreateUser(User user);
        bool UserExists(int userId);
    }
}