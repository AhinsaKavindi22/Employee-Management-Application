﻿using Employee_Management_Web_API.Models;

namespace Employee_Management_Web_API.DTO
{
    public class EmployeeDTO
    {
  
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Age { get; set; }
            public int IsActive { get; set; }
            public string? DepartmentName { get; set; }

    }
}
