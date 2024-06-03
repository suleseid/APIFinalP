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
    public class AdmissionController : ControllerBase
    {
        //GET: api/Admission
        //This endpoint is typically returns all of the entities (admission) from the database
        //Select * from Admission
        //Constractor will initialize properly
        string connectionString;

        public AdmissionController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/admissions
        [HttpGet]
        public ActionResult<List<Admission>> GetAllAdmissions()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Admission> admissions = connection.Query<Admission>("Select * From Hospital.Admission").ToList();
            return Ok(admissions);
        }
        //Get with an id
        //Api/Admission/id
        [HttpGet("{id}")]
        public ActionResult<Admission> GetAdmissions(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Admission admissions = connection.QueryFirstOrDefault<Admission>(
                "SELECT * From Hospital.Admission WHERE Admission_Id = @Id", new { Id = id });
            //Check to see that we got a admission by Id
            if (admissions == null)
            {
                //if no admission return a 404
                return NotFound();
            }
            //return admissions
            return Ok(admissions);
        }

        //Post-Create
        //Put-Update
        //Delete
        [HttpPost]
        public ActionResult<Admission> CreateAdmission(Admission admission)
        {
            if (admission.Patient_Id < 1)
            {
                return BadRequest(new { message = "Invalid Patient_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = admission.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }

            try
            {
                Admission newAdmission = connection.QuerySingle<Admission>(
                    "INSERT INTO Hospital.Admission (Patient_Id, AdmissionDate, DischargeDate) " +
                    "OUTPUT INSERTED.* VALUES (@Patient_Id, @AdmissionDate, @DischargeDate);", admission);

                return Ok(newAdmission);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = "An error occurred while creating the admission.", error = ex.Message });
            }
        }

        //Update operation
        //PUT api/Admission/id
        [HttpPut("{id}")]
        public ActionResult<Admission> UpdateAdmission(int id, Admission admission)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            admission.Admission_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(admission);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Admission SET Patient_Id =@Patient_Id, AdmissionDate = @AdmissionDate, DischargeDate =@DischargeDate " +
                " WHERE Admission_Id =@Admission_Id ", admission);
            if (rowAffected == 0)
            {
                return NotFound();
            }
            return Ok(admission);
        }

        //Delete operation
        //Delete api/Admission/id
        [HttpDelete("{id}")]
        public ActionResult DeleteAdmission(int id)
        {
            if(id < 1)
            {
                return BadRequest();
            }
            using SqlConnection connection = new SqlConnection( connectionString);
            int rawAffected = connection.Execute("DELETE FROM Hospital.Admission WHERE Admission_Id = @Id", new {Id = @id });
            if (rawAffected == 0)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
