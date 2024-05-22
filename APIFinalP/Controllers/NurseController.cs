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
            using SqlConnection connection = new SqlConnection(connectionString);
            return Ok(nurse);

            try
            {
                Nurse newNurse = connection.QuerySingle<Nurse>(
                    "INSERT INTO Hospital.Nurse(First Name, Last Name, Department_Id, Patient_Id) " +
                    "VALUES (@First Name, @Last Name, @Department_Id, @Patient_Id); SELLECT * FROM Hospital.Nurse " +
                    "WHERE Nurse_Id = SCOPE_IDENTITY(); ", nurse);
                return Ok(newNurse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }
        //PUT api/Nurse/id
        [HttpPut("{id}")]
        public ActionResult<Nurse> UpdateNurse(int id, Nurse nurse)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            nurse.Nurse_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(nurse);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Nurse SET First Name =@First Name, Last Name =@Last Name, Department_Id =@Department_Id, Patient_Id =@Patient_Id " +
                " WHERE Nurse_Id =@Nurse_Id ", nurse);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(nurse);
        }
    }
}
