using Employee_Management_Web_API.Models;

namespace Employee_Management_Web_API.DTO
{
    public class DepartmentDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<EmployeeDTO>? Employees { get; set; }
    }
}
