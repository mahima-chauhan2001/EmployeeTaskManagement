using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.Controllers
{
    public class TaskController
    {
        [Route("api/tasks")]
        [ApiController]
        [Authorize]
        public class TasksController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public TasksController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public IActionResult GetAllTasks()
            {
                // This endpoint is accessible only by authenticated users
                var tasks = _context.Tasks.ToList();
                return Ok(tasks);
            }

            [HttpPost]
            [Authorize(Roles = "Admin, Manager")] // Only Admin or Manager can create tasks
            public IActionResult CreateTask([FromBody] TasksModel task)
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetAllTasks), new { id = task.Id }, task);
            }
        }
    }
}
