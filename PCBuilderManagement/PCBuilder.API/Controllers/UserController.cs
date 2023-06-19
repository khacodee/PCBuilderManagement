using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Services.DTO;
using PCBuilder.Services.Service;

namespace PCBuilder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            var User = await _userServices.GetUsersAsync();
            return Ok(User);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userServices.GetUserByIdAsync(id);

            if (!user.Success)
            {
                return NotFound(user);
            }

            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            var createdUser = await _userServices.CreateUserAsync(userDTO);
            return createdUser.Success ? CreatedAtAction(nameof(GetUserById), new { id = createdUser.Data.Id }, createdUser) : (ActionResult)BadRequest(createdUser);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            var updatedUser = await _userServices.UpdateUserAsync(id, userDTO);
            if (!updatedUser.Success)
            {
                return NotFound(updatedUser);
            }
            return Ok(updatedUser);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deletedUser = await _userServices.DeleteUserAsync(id);
            if (!deletedUser.Success)
            {
                return NotFound(deletedUser);
            }
            return Ok(deletedUser);
        }
    }
}
