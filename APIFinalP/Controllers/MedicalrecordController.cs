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
            using SqlConnection connection = new SqlConnection(connectionString);
            return Ok(medicalrecord);

            try
            {
                Medicalrecord newMedicalrecord = connection.QuerySingle<Medicalrecord>(
                    "INSERT INTO Hospital.Medicalrecord(Patient_Id, Doctor_Id, Diagnosis, Treatment ) " +
                    "VALUES (@Patient_Id, @Doctor_Id, @Diagnosis, @Treatment  ); SELLECT * FROM Hospital.Medicalrecord " +
                    "WHERE Medicalrecord_Id = SCOPE_IDENTITY(); ", medicalrecord);
                return Ok(newMedicalrecord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }
        //PUT api/Medicalrecord/id
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
                "UPDATE Hospital.Medicalrecord SET Patient = @Patient_Id, Doctor= @Doctor_Id, Diagnosis =@Diagnosis, Treatment =@Treatment  " +
                " WHERE Medicalrecord_Id =@Medicalrecord_Id ", medicalrecord);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(medicalrecord);
        }
    }
}
