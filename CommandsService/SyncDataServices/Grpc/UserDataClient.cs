using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using UserService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class UserDataClient : IUserDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public UserDataClient(IConfiguration configuration, IWebHostEnvironment environment, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _environment = environment;
        }
        public IEnumerable<User>? ReturnAllUsers()
        {
            Console.WriteLine($"--> Calling GRPC Service to get all initial users {_configuration["GrpcPlatform"]}");
            GrpcChannel channel;

            //If it's a Development environment any ssl certificat will be accepted. This is a workaround for the Ubuntu OS which has some problems with the dotnet generated certificates
            if (_environment.IsDevelopment())
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                channel = GrpcChannel.ForAddress(_configuration["GrpcUser"], new GrpcChannelOptions { HttpHandler = handler });
            }
            //If it's Production we must use mandatory ssl certificates 
            else
            {
                channel = GrpcChannel.ForAddress(_configuration["GrpcUser"]);
            }
            var client = new GrpcUser.GrpcUserClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllUsers(request);
                return _mapper.Map<IEnumerable<User>>(reply.Users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}