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
        string connectionString;

        public PatientController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
        }

        [HttpGet]
        public ActionResult<List<Patient>> GetAllPatients()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Patient> patients = connection.Query<Patient>("Select * From Hospital.Patient").ToList();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatients(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * From Hospital.Patient WHERE Patient_Id = @Id", new { Id = id });
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public ActionResult<Patient> CreatePatients(Patient patient)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                Patient newPatient = connection.QuerySingle<Patient>(
                    "INSERT INTO Hospital.Patient (FirstName, LastName, Age, Gender, Address) " +
                    "OUTPUT INSERTED.* VALUES (@FirstName, @LastName, @Age, @Gender, @Address);", patient);
                return Ok(newPatient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }
        //Update operations
        //PUT api/Patient/id
        [HttpPut("{id}")]
        public ActionResult<Patient> UpdatePatient(int id, Patient patient)
        {
            if (id < 1)
            {
                return NotFound();
            }
            patient.Patient_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(Patient);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Patient SET FirstName =@FirstName, LastName =@LastName, Age = @Age, Gender =@Gender, Address =@Address " +
                " WHERE Patient_Id =@Patient_Id ", patient);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(patient);
        }

        //Delete opreation
        //Delete Api/Patient/id
        [HttpDelete("{id}")]

        public ActionResult DeletePatient(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rowAffected = connection.Execute("DELETE FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = id });

            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
