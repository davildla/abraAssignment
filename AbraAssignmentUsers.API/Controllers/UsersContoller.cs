using AbraAssignmentUsers.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AbraAssignmentUsers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersContoller : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersContoller(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        { // for developer
            var users = await userRepository.GetAllAsync();
            var res = mapper.Map<List<Models.DTO.User>>(users); // need to make a profile

            return Ok(res);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetNewUser")]
        public async Task<IActionResult> GetNewUser(Guid id)
        {
            var user = await userRepository.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var res = mapper.Map<Models.DTO.User>(user);
                return Ok(res);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTodoAsync(Models.DTO.AddUserRequest addUserRequest)
        {
            // Request(DTO) to Domain modal
            var user = new Models.Domain.User()
            {
                Name = addUserRequest.Name,
                Email = addUserRequest.Email,
                Gender = addUserRequest.Gender,
                Phone = addUserRequest.Phone,
                Country = addUserRequest.Country,
            };

            // pass details to Reposetory
            user = await userRepository.AddUserAsync(user);

            // Convert back to DTO
            var res = mapper.Map<Models.DTO.User>(user);

            return CreatedAtAction(nameof(GetNewUser), new { id = res.Id }, res);
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateUserData([FromRoute] Guid id, [FromBody] Models.DTO.UpdateUserRequest updateUserRequest)
        {
            // Convert DTO to Domain model
            var user = new Models.Domain.User()
            {
                Name = updateUserRequest.Name,
                Email = updateUserRequest.Email,
                Gender = updateUserRequest.Gender,
                Phone = updateUserRequest.Phone,
                Country = updateUserRequest.Country,
            };

            // Update Region using reposetory
            user = await userRepository.UpdateUserAsync(id, user);

            // if null return NotFound
            if (user == null)
            {
                return NotFound();
            }

            // Convert to DTO
            var res = mapper.Map<Models.DTO.User>(user);

            // return Ok response
            return Ok(res);
        }
    }
}
