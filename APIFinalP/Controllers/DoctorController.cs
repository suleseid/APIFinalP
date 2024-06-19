using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using APIFinalP.Models;

namespace APIFinalP.Controllers
{
    //This is the route to the controller
    [Route("api/[controller]")] //This attribute defines the route template for the controller.
    [ApiController] //This attribute indicates that the controller responds to web API requests.

    //We inherit from ControllerBase
    public class DoctorController : ControllerBase
    {
        //GET: api/Doctor
        //This endpoint is typically returns all of the entities (doctor) from the database
        //Select * from Doctor
        //Constractor will initialize properly.
        string connectionString; //This field used to connect to the database.

        //This is constructor.
        //The constractor asign the field.
        public DoctorController(IConfiguration configuration)
        {
            //This assigns our project (APIFinalP) to the connectionString field. 
            connectionString = configuration.GetConnectionString("APIFinalP");
            //connectionString:- is retrieving the connection string from the configuration,
            //that we decouple the connection string from our code. 
        }
        //get
        //api/doctors
        [HttpGet]
        //Inside the ActionResult there is a generic which is acual shape of the data.
        public ActionResult<List<Doctor>> GetAllDoctors()
        {
            //We gonna fire up the SqleConnection.
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Doctor>doctors = connection.Query<Doctor>("Select * From Hospital.Doctor").ToList();
            return Ok(doctors);
        }
        //Get with an id
        //Api/Doctor/id
        [HttpGet("{id}")]
        public ActionResult<Doctor> GetDoctors(int id)
        {
            //Lets initializes a new SqlConnection.
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Doctor doctors = connection.QueryFirstOrDefault<Doctor>(
                "SELECT * From Hospital.Doctor WHERE Doctor_Id = @Id", new { Id = id });
            //Check to see that we got a doctor by Id
            if (doctors == null)
            {
                //if no doctor return a 404
                return NotFound(new {Message = "Doctor Not Found!"});
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
            //Make sure if department_id or Patient_id exists.If not I dont gonna bather my data.
            if (doctor.Department_Id < 1 || doctor.Patient_Id < 1)
            {
                return BadRequest(new { message = "Invalid Department_Id or Patient_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Query the department by the doctor's Department_Id
           //'doctor.Department_Id' is the ID of the department to which the doctor belongs.
            Department department = connection.QueryFirstOrDefault<Department>(
                "SELECT * FROM Hospital.Department WHERE Department_Id = @Id", new { Id = doctor.Department_Id });//the ID of the department to which the doctor belongs.
            // Check if the department exists
            if (department == null)
            {
                return BadRequest(new { Message = "Department not found." });
            }

            //Query the patient by the doctor's Patient_Id
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = doctor.Patient_Id });
            // Check if the patient exists
            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }
            //lets create newDoctor.
            try
            {
                Doctor newDoctor = connection.QuerySingle<Doctor>(
                    "INSERT INTO Hospital.Doctor (FirstName, LastName, Specialization, Department_Id, Patient_Id) " +
                    "OUTPUT INSERTED.* VALUES (@FirstName, @LastName, @Specialization, @Department_Id, @Patient_Id);", doctor);

                return Ok(newDoctor); //Save new Doctor and send the message to the client. 
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
