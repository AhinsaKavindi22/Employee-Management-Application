using System.Text.Json.Serialization;

namespace Employee_Management_Web_API.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [JsonIgnore]
        public List<Employee>? Employees { get; set; }

    }
}
