using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.Services.TaskService
{
    public interface ITaskService
    {
        Task<IEnumerable<TasksModel>> GetAllTasksAsync();
        Task<TasksModel> CreateTaskAsync(TasksModel task);
        Task<TasksModel> UpdateTaskAsync(int id, TasksModel taskUpdate);
        Task<TasksModel> DeleteTaskAsync(int id);
    }
}
