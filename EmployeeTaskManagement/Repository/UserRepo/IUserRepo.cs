using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.Repository.UserRepo
{
    public interface IUserRepo
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel> CreateEmployeeAsync(UserModel userModel);
        Task<UserModel> UpdateEmployeeAsync(int userId, UserModel userModel);
        Task<bool> DeleteEmployeeAsync(int userId);
        Task<UserModel> GetUserByIdAsync(int? userId);
    }
}
