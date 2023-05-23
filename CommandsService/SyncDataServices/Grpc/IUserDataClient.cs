using CommandsService.Models;

namespace CommandsService.SyncDataServices.Grpc
{
    public interface IUserDataClient
    {
        IEnumerable<User>? ReturnAllUsers();
    }
}