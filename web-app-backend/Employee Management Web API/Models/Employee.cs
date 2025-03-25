using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Employee_Management_Web_API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Age { get; set; }

        public int IsActive { get; set; }

        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]

        [JsonIgnore]
        public Department? Department { get; set; }
    }
}
