using Azure; 
using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.Services.TaskService;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 

namespace EmployeeTaskManagement.Controllers
{ 
    //[Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {  
        private readonly ITaskService _taskService; 
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }   

        [Authorize]       
        [HttpGet("GetTasks")]    
        //[Route("api/[controller]")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
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
            var createdTask = await _taskService.CreateTaskAsync(task);
            if (createdTask == null)
            {
                return BadRequest("Error while creating the task.");
            }
            return CreatedAtAction(nameof(CreateTask), new { id = createdTask.Id }, createdTask);

            //UserModel assignedFromUser = null;
            //if (task == null)
            //{
            //    return BadRequest("Task data is required.");
            //}

            //if (string.IsNullOrWhiteSpace(task.Title) || string.IsNullOrWhiteSpace(task.Description))
            //{
            //    return BadRequest("Title and Description are required.");
            //} 
            //if (task.AssignedFromId == 0)
            //{
            //    return BadRequest("AssignedFromId is required.");
            //}

            ////if (task.AssignedToId == 0)
            ////{
            ////    return BadRequest("AssignedToId is required.");
            ////}

            //// Check if the admin user with AssignedFromId exists
            //if (task.AssignedFromId != null  )
            //{
            //      assignedFromUser = await _context.UserModels
            //                                     .Where(u => u.UserId == task.AssignedFromId && u.Role == "Admin")
            //                                     .FirstOrDefaultAsync();
            //}
           

            //if (assignedFromUser == null)
            //{
            //    return BadRequest("AssignedFrom user (admin) not found.");
            //} 
            //task.AssignedFrom = assignedFromUser; 
            //// Set CreatedDate and DueDate if not already set
            //if (task.CreatedDate == null)
            //{
            //    task.CreatedDate = DateTime.UtcNow;
            //}
           
            //if (task.DueDate == default)
            //{
            //    task.DueDate = DateTime.UtcNow.AddDays(7);  
            //}
             
            //if(task.Status == null)
            //{
            //    task.Status = "Pending";
            //}
            //_context.Tasks.Add(task);
            //await _context.SaveChangesAsync();
             
            //return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
        }

        [HttpPut("updateTask")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TasksModel taskUpdate)
        { 
            if (taskUpdate == null)
            {
                return BadRequest("Task data is required.");
            }
            try
            {
                var updatedTask = await _taskService.UpdateTaskAsync(id, taskUpdate);
                return Ok(new { message = "Task updated successfully", updatedTask });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });   
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
            //var task = await _context.Tasks.Where(t => t.Id == id).FirstOrDefaultAsync();
            //if (task == null)
            //{
            //    return NotFound(new { message = "Task not found" });  
            //}
             
            //task.Title = taskUpdate.Title ?? task.Title;   
            //task.Description = taskUpdate.Description ?? task.Description;
            //task.Status = taskUpdate.Status ?? task.Status;
            //task.AssignedFromId = taskUpdate.AssignedFromId ?? task.AssignedFromId;
            //task.AssignedToId = taskUpdate.AssignedToId ?? task.AssignedToId;
             
            //await _context.SaveChangesAsync();

            //return Ok(new { message = "Task updated successfully", task });
        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var deletedTask = await _taskService.DeleteTaskAsync(id);
                return Ok(new { message = "Task successfully deleted", deletedTask });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });   
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}

