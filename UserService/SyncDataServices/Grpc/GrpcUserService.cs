using AutoMapper;
using Grpc.Core;
using UserService.Data;

namespace UserService.SyncDataServices.Grpc
{
    public class GrpcUserService : GrpcUser.GrpcUserBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;

        public GrpcUserService(IUserRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<UserResponse> GetAllUsers(GetAllRequest request, ServerCallContext context)
        {
            var response = new UserResponse();
            var users = _repository.GetAllUsers();

            foreach (var user in users)
            {
                response.Users.Add(_mapper.Map<GrpcUserModel>(user));
            }

            return Task.FromResult(response);
        }
    }
}