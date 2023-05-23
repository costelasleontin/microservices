using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Platforms.Add(plat);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.ExternalID == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault()!;
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId).OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Add(user);
        }

        public bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        public IEnumerable<Command> GetCommandsForUserAndPlatform(int userId, int platformId)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Id == userId);
            Console.WriteLine($"-->username:{user?.FirstName}");
            if (user == null)
            {
                Console.WriteLine("--> The user for the command wasn't found");
                //return empty List if the user wasn't found or we could throw and exception
                return new List<Command>();
            }
            else
            {
                var commands = _context.Commands.Where(c => c.PlatformId == platformId).OrderBy(c => c.Platform.Name).ToList();
                Console.WriteLine($"firstcmd:{commands?.First().Id}");
                Console.WriteLine($"firstcmdhowto:{commands?.First().HowTo}");
                List<Command> newcommands = new();
                if (commands != null)
                {
                    newcommands = commands.ToList();
                    if (user.IsAdmin || user.IsWheel)
                    {
                        if (user.IsAdmin)
                        {
                            foreach (var cmd in newcommands)
                            {
                                cmd.HowTo += $".This command requires on Windows to be run with administrator privileges by the user {user.AccountName}";
                            }
                        }
                        if (user.IsWheel)
                        {
                            foreach (var cmd in newcommands)
                            {
                                cmd.HowTo += $".This command requires on Linux and MacOS to be run with sudo for the user {user.AccountName}";
                            }
                        }
                    }
                    else
                    {
                        foreach (var cmd in newcommands)
                        {
                            cmd.HowTo += $".This command requires normal privileges to be used  by the user {user.AccountName}";
                        }
                    }
                }
                //return the processed commands
                return newcommands;

            }
        }

        public bool SaveChange()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}