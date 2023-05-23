using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
        // private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public UsersController(
            IUserRepo repository,
            IMapper mapper,
            // ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient
            )
        {
            _repository = repository;
            _mapper = mapper;
            // _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetAllUsers()
        {
            Console.WriteLine("--> Getting All Users....");

            var userItem = _repository.GetAllUsers();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItem));
        }

        [HttpGet("admins")]
        public ActionResult<IEnumerable<UserReadDto>> GetAllAdmins()
        {
            Console.WriteLine("--> Getting All Admin Users....");

            var userItem = _repository.GetAllAdmins();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItem));
        }

        [HttpGet("wheels")]
        public ActionResult<IEnumerable<UserReadDto>> GetAllWheelUsers()
        {
            Console.WriteLine("--> Getting All Wheel Users....");

            var userItem = _repository.GetAllAdmins();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItem));
        }

        [HttpGet("nonactive")]
        public ActionResult<IEnumerable<UserReadDto>> GetAllNonActiveUsers()
        {
            Console.WriteLine("--> Getting All Nonactive Users....");

            var userItem = _repository.GetAllNonActiveUsers();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItem));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserReadDto> GetUserById(int id)
        {
            var userItem = _repository.GetUserById(id);
            if (userItem != null)
            {
                return Ok(_mapper.Map<UserReadDto>(userItem));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<UserReadDto> CreateUser(UserCreateDto userCreateDto)
        {
            var userModel = _mapper.Map<User>(userCreateDto);
            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<UserReadDto>(userModel);

            // Send Async Message
            try
            {
                var userPublishedDto = _mapper.Map<UserPublishedDto>(userReadDto);
                userPublishedDto.Event = "User_Published";
                _messageBusClient.PublishNewUser(userPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }
    }
}