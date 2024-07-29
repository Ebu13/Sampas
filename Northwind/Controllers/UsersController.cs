using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Services;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGenericService<User> _userService;


        public UsersController(IGenericService<User> userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> GetUserByCredentials([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login request.");
            }

            var users = await _userService.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            await _userService.UpdateAsync(id, user);
            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
