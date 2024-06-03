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
    public class DoctorController : ControllerBase
    {
        //GET: api/Doctor
        //This endpoint is typically returns all of the entities (doctor) from the database
        //Select * from Doctor
        //Constractor will initialize properly
        string connectionString;

        public DoctorController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/doctors
        [HttpGet]
        public ActionResult<List<Doctor>> GetAllDoctors()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Doctor>doctors = connection.Query<Doctor>("Select * From Hospital.Doctor").ToList();
            return Ok(doctors);
        }
        //Get with an id
        //Api/Doctor/id
        [HttpGet("{id}")]
        public ActionResult<Doctor> GetDoctors(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Doctor doctors = connection.QueryFirstOrDefault<Doctor>(
                "SELECT * From Hospital.Doctor WHERE Doctor_Id = @Id", new { Id = id });
            //Check to see that we got a doctor by Id
            if (doctors == null)
            {
                //if no doctor return a 404
                return NotFound();
            }
            //return doctors
            return Ok(doctors);
        }

        //Post-Create
        //Put-Update
        //Delete
        [HttpPost]
        public ActionResult<Doctor> CreateDoctor(Doctor doctor)
        {
            if (doctor.Department_Id < 1 || doctor.Patient_Id < 1)
            {
                return BadRequest(new { message = "Invalid Department_Id or Patient_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the department exists
            Department department = connection.QueryFirstOrDefault<Department>(
                "SELECT * FROM Hospital.Department WHERE Department_Id = @Id", new { Id = doctor.Department_Id });

            if (department == null)
            {
                return BadRequest(new { message = "Department not found." });
            }

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = doctor.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }

            try
            {
                Doctor newDoctor = connection.QuerySingle<Doctor>(
                    "INSERT INTO Hospital.Doctor (FirstName, LastName, Specialization, Department_Id, Patient_Id) " +
                    "OUTPUT INSERTED.* VALUES (@FirstName, @LastName, @Specialization, @Department_Id, @Patient_Id);", doctor);

                return Ok(newDoctor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = "An error occurred while creating the doctor.", error = ex.Message });
            }
        }


        //Update opreations
        //PUT api/Doctor/id
        [HttpPut("{id}")]
        public ActionResult<Doctor> UpdateDoctor(int id, Doctor doctor)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            doctor.Doctor_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(doctor);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Doctor SET FirstName =@FirstName, LastName =@LastName, Specialization = @Specialization, Department_Id =@Department_Id, Patient_Id =@Patient_Id " +
                " WHERE Doctor_Id =@Doctor_Id ", doctor);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(doctor);
        }
        //Delete operation
        //Delete api/Doctor/id
        [HttpDelete("{id}")]

        public ActionResult DeleteDoctor(int id) 
        {
            if(id < 1)
            {
                return NotFound();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rowAffected = connection.Execute("DELETE FROM Hospital.Doctor WHERE Doctor_Id = @Id", new {Id = id });
            
            if(rowAffected == 0)
            {
                return BadRequest();
            }
             return Ok();
        }
    }
}
