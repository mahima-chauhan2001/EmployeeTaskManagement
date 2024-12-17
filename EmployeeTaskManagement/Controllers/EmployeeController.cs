using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmployeeTaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserService _userService;
        public EmployeeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsers")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("CreateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> CreateEmployee([FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest(new { message = "User data is required." });
            }
            await _userService.CreateEmployeeAsync(userModel);

            return Ok(new { message = "Task updated successfully", userModel });
        }

        [HttpPut("UpdateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> UpdateEmployee(int userId, [FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest(new { message = "User data not found" });
            }
            var updatedUser = await _userService.UpdateEmployeeAsync(userId, userModel);
            if (updatedUser == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User updated successfully", user = updatedUser }); 
        }

        [HttpDelete("DeleteEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEmployee([FromQuery] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { message = "Invalid user ID" });
            }
            var result = await _userService.DeleteEmployeeAsync(userId);

            if (result == null)
            {
                return BadRequest(new { message = "User data not found" });
            }
            return Ok(new { message = "User successfully deleted" });
             
        }
    }
}
