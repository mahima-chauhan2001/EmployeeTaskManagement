using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Repository.UserRepo
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.UserModels.ToListAsync();
        }

        public async Task<UserModel> CreateEmployeeAsync(UserModel userModel)
        {
            await _context.UserModels.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<UserModel> UpdateEmployeeAsync(int userId, UserModel userModel)
        { 
            var user = await _context.UserModels.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return null;   
            }
             
            user.Email = userModel.Email;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Role = userModel.Role;
             
            await _context.SaveChangesAsync();

            return user;   
        }

        public async Task<bool> DeleteEmployeeAsync(int userId)
        { 
            var user = await _context.UserModels.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                return false;  
            } 
            _context.UserModels.Remove(user);
             
            await _context.SaveChangesAsync();

            return true;   
        }

        public async Task<UserModel> GetUserByIdAsync(int? userId)
        {
            return await _context.UserModels.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
