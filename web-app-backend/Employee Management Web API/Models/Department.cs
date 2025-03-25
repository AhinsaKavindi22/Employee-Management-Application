namespace Employee_Management_Web_API.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<Employee>? Employees { get; set; }

    }
}
