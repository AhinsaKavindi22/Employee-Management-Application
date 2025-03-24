namespace Employee_Management_Web_API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Age { get; set; }

        public int IsActive { get; set; }
    }
}
