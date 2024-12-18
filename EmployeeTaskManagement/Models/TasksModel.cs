using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Models
{
    public class TasksModel
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int? AssignedFromId { get; set; } // Foreign key for the user who assigned the task
        public UserModel? AssignedFrom { get; set; }  // Navigation property to the user who assigned the task

        public int? AssignedToId { get; set; } // Foreign key for the assigned user
        public UserModel? AssignedTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
    }
} 