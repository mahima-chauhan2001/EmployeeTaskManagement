using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task CopyUserDataToUserModel(ApplicationUser applicationUser)
        {
            var nameParts = applicationUser.UserName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            string lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            // Create a new UserModel object
           if(int.TryParse(applicationUser.Id, out int userId))
            {
                var userModel = new UserModel
                {
                    UserId = userId,
                    FirstName = firstName, // Assuming you have custom properties
                    LastName = lastName,   // Assuming you have custom properties
                    Email = applicationUser.Email,
                    Password = applicationUser.PasswordHash,       // Custom property
                };
                _context.UserModels.Add(userModel);
                await _context.SaveChangesAsync();
            }
            
           
        }
    }
}
