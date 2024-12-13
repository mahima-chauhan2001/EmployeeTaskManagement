using Azure;
using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeTaskManagement.Controllers
{ 
    //[Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    { 
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        public TasksController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {  
            _context = context;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin")]       
        [HttpGet("GetTasks")]    
        //[Route("api/[controller]")]
        public async Task<IActionResult> GetAllTasks()
        {          
            var tasks = _context.Tasks.ToList();
            return Ok(tasks);
        } 

        [Authorize(Roles = "Admin")]
        [HttpPost("admin/createTasks")]
        public async Task<IActionResult> CreateTask([FromBody] TasksModel task)
        {
            if (task == null)
            {
                return BadRequest("Task data is required.");
            }

            if (string.IsNullOrWhiteSpace(task.Title) || string.IsNullOrWhiteSpace(task.Description))
            {
                return BadRequest("Title and Description are required.");
            }

            // Validate AssignedFromId and AssignedToId
            //if (task.AssignedFromId == 0)
            //{
            //    return BadRequest("AssignedFromId is required.");
            //}

            //if (task.AssignedToId == 0)
            //{
            //    return BadRequest("AssignedToId is required.");
            //}

            // Check if the admin user with AssignedFromId exists
            //var assignedFromUser = await _context.UserModels
            //                                      .Where(u => u.UserId == task.AssignedFromId && u.Role == "Admin")
            //                                      .FirstOrDefaultAsync();

            //if (assignedFromUser == null)
            //{
            //    return BadRequest("AssignedFrom user (admin) not found.");
            //}
            //var assignedToUser = await _context.UserModels
            //                              .FirstOrDefaultAsync(u => u.FirstName == task.AssignedTo.FirstName &&
            //                                                        u.LastName == task.AssignedTo.LastName &&
            //                                                        u.Email == task.AssignedTo.Email && u.UserId == task.AssignedToId);

            //var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (assignedToUser == null)
            //{
            //    return BadRequest("AssignedTo user not found.");
            //}

            //// Populate task with the users (AssignedFrom and AssignedTo)
            //task.AssignedFrom = assignedFromUser;
            //task.AssignedTo = assignedToUser;

            // Set CreatedDate and DueDate if not already set
            if (task.CreatedDate == null)
            {
                task.CreatedDate = DateTime.UtcNow;
            }
           
            if (task.DueDate == default)
            {
                task.DueDate = DateTime.UtcNow.AddDays(7); // Example: Set default due date to 7 days from now
            }
             
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
             
            return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
        }

        [HttpPut("updateTask")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TasksModel taskUpdate)
        { 
            if (taskUpdate == null)
            {
                return BadRequest("Task data is required.");
            }
             
            var task = await _context.Tasks.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (task == null)
            {
                return NotFound(new { message = "Task not found" });  
            }
             
            task.Title = taskUpdate.Title ?? task.Title;   
            task.Description = taskUpdate.Description ?? task.Description;
            task.Status = taskUpdate.Status ?? task.Status;
            task.AssignedToId = taskUpdate.AssignedToId ?? task.AssignedToId;
             
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task updated successfully", task });
        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        { 
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new { message = "Task not found" });  
            } 
            _context.Tasks.Remove(task);
             
            await _context.SaveChangesAsync();

            return Ok("successfully deleted");
        }
    }
}

