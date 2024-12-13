using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
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
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> GetUsers()
        {
            return Ok(await _context.UserModels.ToListAsync());
        }

        [HttpPost("CreateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> CreateEmployee([FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest(new { message = "User data is required." });
            }

            _context.UserModels.Add(userModel);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task updated successfully", userModel });
        }

        [HttpPut("UpdateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> UpdateEmployee(int Userid, [FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest(new { message = "User data not found" });
            }
            var user = await _context.UserModels.Where(x => x.UserId == Userid).FirstOrDefaultAsync();

            user.Email = userModel.Email;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Role = userModel.Role;
             
            await _context.SaveChangesAsync();
            return Ok(new { message = "Task updated successfully", user });
        }

        [HttpDelete("DeleteEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEmployee(int Userid)
        {           
            var user = await _context.UserModels.Where(x=> x.UserId == Userid).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest(new { message = "User data not found" });
            }
            var u = await _context.UserModels.FindAsync(Userid);
            _context.UserModels.Remove(user);

            return Ok(new { message = "successfully deleted" });
        }
    }
}
