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
    public class NurseController : ControllerBase
    {
        //GET: api/Nurse
        //This endpoint is typically returns all of the entities (nurse) from the database
        //Select * from Nurse
        //Constractor will initialize properly
        string connectionString;

        public NurseController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/nurses
        [HttpGet]
        public ActionResult<List<Nurse>> GetAllNurses()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Nurse> nurses = connection.Query<Nurse>("Select * From Hospital.Nurse").ToList();
            return Ok(nurses);
        }
        //Get with an id
        //Api/Nurse/id
        [HttpGet("{id}")]
        public ActionResult<Nurse> GetNurses(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Nurse nurses = connection.QueryFirstOrDefault<Nurse>(
                "SELECT * From Hospital.Nurse WHERE Nurse_Id = @Id", new { Id = id });
            //Check to see that we got a nurse by Id
            if (nurses == null)
            {
                //if no nurse return a 404
                return NotFound();
            }
            //return nurses
            return Ok(nurses);
        }

        //Post-Create
        //Put-Update
        //Delete
        [HttpPost]
        public ActionResult<Nurse> CreateNurse(Nurse nurse)
        {
            if (nurse.Department_Id < 1 || nurse.Patient_Id < 1)
            {
                return BadRequest(new { message = "Invalid Department_Id or Patient_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the department exists
            Department department = connection.QueryFirstOrDefault<Department>(
                "SELECT * FROM Hospital.Department WHERE Department_Id = @Id", new { Id = nurse.Department_Id });

            if (department == null)
            {
                return BadRequest(new { message = "Department not found." });
            }

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = nurse.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }

            try
            {
                Nurse newNurse = connection.QuerySingle<Nurse>(
                    "INSERT INTO Hospital.Nurse (FirstName, LastName, Department_Id, Patient_Id) " +
                    "OUTPUT INSERTED.* VALUES (@FirstName, @LastName, @Department_Id, @Patient_Id);", nurse);

                return Ok(newNurse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = "An error occurred while creating the nurse.", error = ex.Message });
            }
        }


        //Update opreations
        //PUT api/Nurse/id
        [HttpPut("{id}")]
        public ActionResult<Nurse> UpdateNurse(int id, Nurse nurse)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            // Ensure the provided IDs are valid
            if (nurse.Department_Id < 1 || nurse.Patient_Id < 1)
            {
                return BadRequest(new { message = "Invalid Department_Id or Patient_Id." });
            }

            nurse.Nurse_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the department exists
            Department department = connection.QueryFirstOrDefault<Department>(
                "SELECT * FROM Hospital.Department WHERE Department_Id = @Id", new { Id = nurse.Department_Id });

            if (department == null)
            {
                return BadRequest(new { message = "Department not found." });
            }

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = nurse.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }

            // Corrected SQL statement without extra parenthesis
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Nurse SET FirstName = @FirstName, LastName = @LastName, Department_Id = @Department_Id, Patient_Id = @Patient_Id " +
                "WHERE Nurse_Id = @Nurse_Id", nurse);

            if (rowAffected == 0)
            {
                return NotFound();
            }

            return Ok(nurse);
        }


        //Delete operation
        //Delete api/Nurse/id
        [HttpDelete("{id}")]

        public ActionResult DeleteNurse(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rowAffected = connection.Execute("DELETE FROM Hospital.Nurse WHERE Nurse_Id = @Id", new { Id = id });

            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }

}
        

