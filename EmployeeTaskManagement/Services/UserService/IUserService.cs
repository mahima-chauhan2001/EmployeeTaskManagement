using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.Services.UserService
{
    public interface IUserService  
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<UserModel> CreateEmployeeAsync(UserModel userModel);
        Task<UserModel> UpdateEmployeeAsync(int userId, UserModel userModel);
        Task<bool> DeleteEmployeeAsync(int userId);
    }
}
