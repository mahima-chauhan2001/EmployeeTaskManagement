using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Repository.TaskRepo
{
    public class TaskRepo : ITaskRepo
    {
        private readonly ApplicationDbContext _context;
        public TaskRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TasksModel>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TasksModel> CreateTaskAsync(TasksModel task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;  
        }

        public async Task<TasksModel> UpdateTaskAsync(int id, TasksModel taskUpdate)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return null;  
            } 
            task.Title = taskUpdate.Title ?? task.Title;
            task.Description = taskUpdate.Description ?? task.Description;
            task.Status = taskUpdate.Status ?? task.Status;
            task.AssignedFromId = taskUpdate.AssignedFromId ?? task.AssignedFromId;
            task.AssignedToId = taskUpdate.AssignedToId ?? task.AssignedToId;
             
            await _context.SaveChangesAsync();

            return task;   
        }

        public async Task<TasksModel> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return null; 
            } 
            _context.Tasks.Remove(task);   
            await _context.SaveChangesAsync();  

            return task;   
        }
    }
}
