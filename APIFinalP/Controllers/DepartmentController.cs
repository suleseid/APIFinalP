using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using APIFinalP.Models;

namespace APIFinalP.Controllers
{
        //This is the route to the controller
        [Route("api/[controller]")]
        [ApiController]
        //We inherit from ControllerBase
        public class DepartmentController : ControllerBase
        {
            //GET: api/department
            //This endpoint is typically returns all of the entities (department) from the database
            //Select * from Department
            //Constractor will initialize properly
            string connectionString;

            public DepartmentController(IConfiguration configuration)
            {
                connectionString = configuration.GetConnectionString("APIFinalP");
                //Console.WriteLine(connectionString);
            }
            //get
            //api/department
            [HttpGet]
            public ActionResult<List<Department>> GetAllDepartments()
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                List<Department> departments = connection.Query<Department>("Select * From Hospital.Department").ToList();
                return Ok(departments);
            }
            //Get with an id
            //Api/department/id
            [HttpGet("{id}")]
            public ActionResult<Department> GetDepartments(int id)
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                // Using parameterized queries to prevent SQL injection attacks
                Department departments = connection.QueryFirstOrDefault<Department>(
                    "SELECT * From Hospital.Department WHERE Department_Id = @Id", new { Id = id });
                //Check to see that we got a department by Id
                if (departments == null)
                {
                    //if no department return a 404
                    return NotFound();
                }
                //return Appointments
                return Ok(departments);
            }

            //Post-Create
            //Put-Update
            //Delete

            [HttpPost]
            public ActionResult<Department> CreateDdepartment(Department department)
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                return Ok(department);

                try
                {
                    Department newDepartment = connection.QuerySingle<Department>(
                        "INSERT INTO Hospital.Department(Name) " +
                        "VALUES (@Name ); SELLECT * FROM Hospital.Department " +
                        "WHERE Department_Id = SCOPE_IDENTITY(); ", department);
                    return Ok(newDepartment);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest();
                }

            }
            //PUT api/Appointment/id
            [HttpPut("{id}")]
            public ActionResult<Department> UpdateDepartment(int id, Department department)
            {
                if (id < 1)
                {
                    return BadRequest();
                }
                department.Department_Id = id;
                using SqlConnection connection = new SqlConnection(connectionString);
                //return Ok(department);
                //PUT- we have to send every information wheether changed or not
                int rowAffected = connection.Execute(
                    "UPDATE Hospital.Department SET Name =@Name " +
                    " WHERE Department_Id =@Department_Id ", department);
                if (rowAffected == 0)
                {
                    return BadRequest();
                }
                return Ok(department);
            }
        }
}
