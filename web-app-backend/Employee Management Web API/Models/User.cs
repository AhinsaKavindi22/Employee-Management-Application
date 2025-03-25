using System.ComponentModel.DataAnnotations;

namespace Employee_Management_Web_API.Models
{
    public class User
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
