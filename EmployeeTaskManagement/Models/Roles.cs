using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        public string? Name { get; set; }
    }
}
