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
    public class MedicalrecordController : ControllerBase
    {
        //GET: api/medicalrecord
        //This endpoint is typically returns all of the entities (medicalrecord) from the database
        //Select * from medicalrecord
        //Constractor will initialize properly
        string connectionString;

        public MedicalrecordController (IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/medicalrecord
        [HttpGet]
        //Inside the ActionResult there is a generic, which is acual shape of the data.
        public ActionResult<List<Medicalrecord>> GetAllMedicalrecords()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Medicalrecord> medicalrecords = connection.Query<Medicalrecord>("Select * From Hospital.Medicalrecord").ToList();
            return Ok(medicalrecords);
        }
        //Get with an id
        //Api/Medicalrecord/id
        [HttpGet("{id}")]
        public ActionResult<Medicalrecord> GetMedicalrecords(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Medicalrecord medicalrecords = connection.QueryFirstOrDefault<Medicalrecord>(
                "SELECT * From Hospital.Medicalrecord WHERE Medicalrecord_Id = @Id", new { Id = id });
            //Check to see that we got a medicalrecord by Id
            if (medicalrecords == null)
            {
                //if no medicalrecord return a 404
                return NotFound();
            }
            //return Medicalrecords
            return Ok(medicalrecords);
        }

        //Post-Create
        //Put-Update
        //Delete

        [HttpPost]
        public ActionResult<Medicalrecord> CreateMedicalrecord(Medicalrecord medicalrecord)
        {
            if (medicalrecord.Patient_Id < 1 || medicalrecord.Doctor_Id < 1)
            {
                return BadRequest(new { Message = "Invalid Patient_Id or Doctor_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = medicalrecord.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { Message = "Patient Not Found." });
            }

            // Check if the Doctor exists
            Doctor doctor = connection.QueryFirstOrDefault<Doctor>(
                "SELECT * FROM Hospital.Doctor WHERE Doctor_Id = @Id", new { Id = medicalrecord.Doctor_Id });

            if (doctor == null)
            {
                return BadRequest(new { message = "Doctor Not Found." });
            }

            try
            {
                Medicalrecord newMedicalrecord = connection.QuerySingle<Medicalrecord>(
                    "INSERT INTO Hospital.Medicalrecord (Patient_Id, Doctor_Id, Diagnosis, Treatment ) " +
                    "OUTPUT INSERTED.* VALUES (@Patient_Id, @Doctor_Id, @Diagnosis, @Treatment );", medicalrecord);

                return Ok(newMedicalrecord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = "An error occurred while creating the Medicalrecord.", error = ex.Message });
            }
        }
        //Update opreations
        //PUT /api/Medicalrecord/id
        [HttpPut("{id}")]
        public ActionResult<Medicalrecord> UpdateMedicalrecord(int id, Medicalrecord medicalrecord)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            medicalrecord.Medicalrecord_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(medicalrecord);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Medicalrecord SET Patient_Id = @Patient_Id, Doctor_Id = @Doctor_Id, Diagnosis = @Diagnosis, Treatment =@Treatment  " +
                "WHERE Medicalrecord_Id = @Medicalrecord_Id ", medicalrecord);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(medicalrecord);
        }

        //Delete operation
        //Delete api/Medicalrecord/id
        [HttpDelete("{id}")]

        public ActionResult DeleteMedicalrecord(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rowAffected = connection.Execute("DELETE FROM Hospital.Medicalrecord WHERE medicalrecord_Id = @Id", new { Id = id });

            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
