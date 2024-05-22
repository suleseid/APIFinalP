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
    public class PatientController : ControllerBase
    {
        //GET: api/Patient
        //This endpoint is typically returns all of the entities (patient) from the database
        //Select * from Patient
        //Constractor will initialize properly
        string connectionString;

        public PatientController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/patients
        [HttpGet]
        public ActionResult<List<Patient>> GetAllPatients()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Patient> patients = connection.Query<Patient>("Select * From Hospital.Patient").ToList();
            return Ok(patients);
        }
        //Get with an id
        //Api/Patient/id
        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatients(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Patient patients = connection.QueryFirstOrDefault<Patient>(
                "SELECT * From Hospital.Patient WHERE Patient_Id = @Id", new { Id = id });
            //Check to see that we got a patient by Id
            if (patients == null)
            {
                //if no patient return a 404
                return NotFound();
            }
            //return doctors
            return Ok(patients);
        }

        //Post-Create
        //Put-Update
        //Delete

        [HttpPost]
        public ActionResult<Patient> CreatePatients(Patient patient)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return Ok(patient);

            try
            {
                Patient newPatient = connection.QuerySingle<Patient>(
                    "INSERT INTO Hospital.Patient(First Name, Last Name, Age,Gender, Address) " +
                    "VALUES (@First Name, @Last Name, @Age, @Gender, @Address); SELLECT * FROM Hospital.Doctor " +
                    "WHERE Patient_Id = SCOPE_IDENTITY(); ", patient);
                return Ok(newPatient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }
        //PUT api/Patient/id
        [HttpPut("{id}")]
        public ActionResult<Patient> UpdatePatient(int id, Patient patient)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            patient.Patient_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(Patient);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Patient SET First Name =@First Name, Last Name =@Last Name, Age = @Age, Gender =@Gender, Address =@Address " +
                " WHERE Patient_Id =@Patient_Id ", patient);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(patient);
        }

    }
}
