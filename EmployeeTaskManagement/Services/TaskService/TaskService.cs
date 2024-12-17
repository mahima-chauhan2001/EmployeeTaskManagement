using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.Repository.TaskRepo;
using EmployeeTaskManagement.Repository.UserRepo;
using System.Threading.Tasks;

namespace EmployeeTaskManagement.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepo _taskRepo;
        private readonly IUserRepo _userRepo;

        public TaskService(ITaskRepo taskRepo, IUserRepo userRepo) {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<TasksModel>> GetAllTasksAsync()
        {
            return await _taskRepo.GetAllTasksAsync();
        }   

        public async Task<TasksModel> CreateTaskAsync(TasksModel task)
        { 
            if (task.AssignedFromId == 0)
            { 
                throw new ArgumentException("AssignedFromId is required.");
            } 
            var assignedFromUser = await _userRepo.GetUserByIdAsync(task.AssignedFromId);
             
            if (assignedFromUser == null || assignedFromUser.Role != "Admin")
            { 
                throw new ArgumentException("AssignedFrom user must be an admin.");
            } 
            task.AssignedFrom = assignedFromUser;
             
            if (task.CreatedDate == null)
            {
                task.CreatedDate = DateTime.UtcNow;
            }

            if (task.DueDate == default)
            {
                task.DueDate = DateTime.UtcNow.AddDays(7);
            }
             
            if (task.Status == null)
            {
                task.Status = "Pending";
            }
             
            return await _taskRepo.CreateTaskAsync(task);
        }

        public async Task<TasksModel> UpdateTaskAsync(int id, TasksModel taskUpdate)
        { 
            var updatedTask = await _taskRepo.UpdateTaskAsync(id, taskUpdate);

            if (updatedTask == null)
            {
                throw new ArgumentException("Task not found.");  
            }

            return updatedTask;  
        }

        public async Task<TasksModel> DeleteTaskAsync(int id)
        {
            var task = await _taskRepo.DeleteTaskAsync(id);

            if (task == null)
            {
                throw new ArgumentException("Task not found.");   
            } 
            return task;   
        }

    }
}
