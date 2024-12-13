using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.CommonService
{
    public class SeedRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public SeedRoleService(ApplicationDbContext context )
        {
            _context = context;
             
        }

        // Method to seed roles
        public async Task SeedRolesAsync()
        {
            var roles = new[] { "Admin", "Employee" }; // Define your roles here

            foreach (var roleName in roles)
            {
                // Check if role already exists in the Roles table
                var roleExist = await _context.Roles
                    .AnyAsync(r => r.Name == roleName);

                if (!roleExist)
                {
                    // Create and add the new role
                    var role = new Roles { Name = roleName };
                    await _context.Roles.AddAsync(role);
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
            }
        }


        //public async Task AssignRoleToUser(IdentityUser user, string role)
        //{
        //    var result = await _userManager.AddToRoleAsync(user, role);

        //    if (result.Succeeded)
        //    {
        //        return result;
        //    }
        //    else
        //    {
        //        // Handle failure
        //    }
        //}
    }
}
