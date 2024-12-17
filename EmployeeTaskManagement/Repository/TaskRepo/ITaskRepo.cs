using EmployeeTaskManagement.Models;
using System.Threading.Tasks;

namespace EmployeeTaskManagement.Repository.TaskRepo
{
    public interface ITaskRepo
    {
        Task<IEnumerable<TasksModel>> GetAllTasksAsync();
        Task<TasksModel> CreateTaskAsync(TasksModel task);
        Task<TasksModel> UpdateTaskAsync(int id, TasksModel taskUpdate);
        Task<TasksModel> DeleteTaskAsync(int id);
    }
}
