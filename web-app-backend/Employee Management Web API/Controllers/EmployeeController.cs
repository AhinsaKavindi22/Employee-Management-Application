using Employee_Management_Web_API.Data;
using Employee_Management_Web_API.DTO;
using Employee_Management_Web_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly EmployeeContext _employeeContext;
        
        public EmployeeController(EmployeeContext employeeContext) 
        { 
            _employeeContext = employeeContext;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        //{
        //    // IEnumrable is used for return list of employees
        //    if (_employeeContext.Employees == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _employeeContext.Employees.ToListAsync();
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (_employeeContext.Employees == null)
            {
                return NotFound();
            }

            var employees = await _employeeContext.Employees
                .Include(emp => emp.Department) // Join Department table
                .Select(emp => new EmployeeDTO
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    Age = emp.Age,
                    IsActive = emp.IsActive,
                    DepartmentName = emp.Department != null ? emp.Department.Name : "No Department" // Handle null department
                })
                .ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            if (_employeeContext.Employees == null)
            {
                return NotFound();
            }

            var employee = await _employeeContext.Employees
                .Include(e => e.Department) // Include the department data
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    IsActive = e.IsActive,
                    DepartmentName = e.Department.Name // Fetching department name
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }


        //[HttpPost]
        //public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        //{
        //    _employeeContext.Employees.Add(employee);
        //    await _employeeContext.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
        //}

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(EmployeeDTO employeeDto)
        {
            if (_employeeContext.Employees == null || _employeeContext.Departments == null)
            {
                return NotFound();
            }

            // Find department by name
            var department = await _employeeContext.Departments
                                .FirstOrDefaultAsync(d => d.Name == employeeDto.DepartmentName);

            if (department == null)
            {
                return BadRequest("Invalid Department Name.");
            }

            // Create Employee with the found DepartmentId
            var employee = new Employee
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                IsActive = employeeDto.IsActive,
                DepartmentId = department.Id // Assign departmentId
            };

            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> EditEmployee(int id, EmployeeDTO employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return BadRequest();
            }

            // Check if departmentName is provided in the DTO
            if (!string.IsNullOrEmpty(employeeDto.DepartmentName))
            {
                // Find the department based on the department name
                var department = await _employeeContext.Departments
                    .FirstOrDefaultAsync(d => d.Name == employeeDto.DepartmentName);

                // If department is not found, return a bad request
                if (department == null)
                {
                    return BadRequest("Department not found");
                }

                // Retrieve the employee from the database
                var employee = await _employeeContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                // Update employee properties
                employee.Name = employeeDto.Name;
                employee.Age = employeeDto.Age;
                employee.IsActive = employeeDto.IsActive;
                employee.DepartmentId = department.Id;  // Set the DepartmentId

                // Mark the employee entity as modified
                _employeeContext.Entry(employee).State = EntityState.Modified;

                try
                {
                    // Save changes to the database
                    await _employeeContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return Ok();
            }
            else
            {
                return BadRequest("Department name is required");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if(_employeeContext.Employees == null)
            {
                return NotFound();
            }

            var employee = await _employeeContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _employeeContext.Employees.Remove(employee);
            await _employeeContext.SaveChangesAsync();

            return Ok();

        }

    }
}
