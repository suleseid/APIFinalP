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
            using SqlConnection connection = new SqlConnection(connectionString);
            return Ok(doctor);

            try
            {
                Doctor newDoctor = connection.QuerySingle<Doctor>(
                    "INSERT INTO Hospital.Doctor(First Name, Last Name, Specialization,Department_Id, Patient_Id) " +
                    "VALUES (@First Name, @Last Name, @Specialization, @Department_Id, @Patient_Id); SELLECT * FROM Hospital.Doctor " +
                    "WHERE Doctor_Id = SCOPE_IDENTITY(); ", doctor);
                return Ok(newDoctor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }
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
                "UPDATE Hospital.Doctor SET First Name =@First Name, Last Name =@Last Name, Specialization = @Specialization, Department_Id =@Department_Id, Patient_Id =@Patient_Id " +
                " WHERE Doctor_Id =@Doctor_Id ", doctor);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(doctor);
        }

    }
}
