using Microsoft.AspNetCore.Mvc;
using TestAppAPI.Data.Respositories.Interfaces;
using TestAppAPI.Models;

namespace TestAppAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("/getusers")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllUsers();
            return new OkObjectResult(users);
        }

        [HttpGet("/getuser/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userRepository.GetById(id);
            return new OkObjectResult(user);
        }

        [HttpPost("/createuser")]
        public async Task<IActionResult> Post(User user)
        {
            await _userRepository.AddUser(user);
            return new OkObjectResult(user);
        }

        [HttpPut("/updateuser/{id}")]
        public async Task<IActionResult> Put(int id, User user)
        {
            await _userRepository.UpdateUser(id, user);
            return new OkResult();
        }

        [HttpDelete("/deleteuser/{id}")]
        public async Task<IActionResult> Delete(int id, User user)
        {
            await _userRepository.DeleteUser(id, user);
            return new OkResult();
        }
    }
}
