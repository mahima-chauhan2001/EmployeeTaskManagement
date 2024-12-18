using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.Repository.UserRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepo  _iUserRepo;

        public UserService(IUserRepo iUserRepo)
        {
            _iUserRepo = iUserRepo;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return await _iUserRepo.GetAllUsersAsync();
        }

        public async Task<UserModel> CreateEmployeeAsync(UserModel userModel)
        {
            return await _iUserRepo.CreateEmployeeAsync(userModel);
        }

        public async Task<UserModel> UpdateEmployeeAsync(int userId, UserModel userModel)
        { 
            return await _iUserRepo.UpdateEmployeeAsync(userId, userModel);
        }

        public async Task<bool> DeleteEmployeeAsync(int userId)
        { 
            return await _iUserRepo.DeleteEmployeeAsync(userId);
        }

    }
}
