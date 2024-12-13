namespace EmployeeTaskManagement.Models
{
    public class CreateTaskModel
    {
        public string? TaskTitle { get; set; }   
        public string? TaskDescription { get; set; }   
        public DateTime CreatedDate { get; set; }   
        public DateTime DueDate { get; set; }   
    }
}
