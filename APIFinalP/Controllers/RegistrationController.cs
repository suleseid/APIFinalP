using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using APIFinalP.Models;
using System.Collections.Generic;

namespace APIFinalP.Controllers
{
    //This is the route to the controller
    [Route("api/[controller]")]
    [ApiController]
    //We inherit from ControllerBase
    public class RegistrationController: ControllerBase
    {
        //GET: api/Registration
        //This endpoint is typically returns all of the entities (Registration) from the database
        //Select * from Registration
        //Constractor will initialize properly
        string connectionString;

        public RegistrationController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
        }

        [HttpGet]
        public ActionResult<List<Registration>> GetAllRegistrations()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Registration> registrations = connection.Query<Registration>("SELECT * FROM Hospital.Registration").ToList();
            return Ok(registrations);
        }

        [HttpGet("{id}")]
        public ActionResult<Registration> GetRegistrations(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Registration registrations = connection.QueryFirstOrDefault<Registration>(
                "SELECT * From Hospital.Registration WHERE Registration_Id = @Id", new { Id = id });
            //Check to see that we got a registration by Id
            if (registrations == null)
            {
                //if no registration return a 404
                return NotFound();
            }
            //return registrations
            return Ok(registrations);
        }

        //Post-Create
        //Put-Update
        //Delete

        [HttpPost]
        public ActionResult<Registration> CreateRegistration(Registration registration)
        {
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the patient exists (optional but recommended for data integrity)
            var patient = connection.QueryFirstOrDefault<Patient>("SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = registration.Patient_Id });
            if (patient == null) 
                return BadRequest(new { message = "Patient Not Found." });

            try
            {
                Registration newRegistration = connection.QuerySingle<Registration>(
                    "INSERT INTO Hospital.Registration (Patient_Id, RegistrationDate, FirstName, LastName ) " +
                    "OUTPUT INSERTED.* VALUES (@Patient_Id, @RegistrationDate, @FirstName, @LastName );", registration);

                return Ok(newRegistration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { message = "An error occurred while creating the registration.", error = ex.Message });
            }
        }

        //Update/Registration/id
        [HttpPut("{id}")]
        public ActionResult<Registration> UpdateRegistration(int id, Registration registration)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            registration.Registration_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the patient exists (optional but recommended for data integrity)
            var patient = connection.QueryFirstOrDefault<Patient>("SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = registration.Patient_Id });
            if (patient == null) 
                return BadRequest(new { message = "Patient not found." });

            int rowAffected = connection.Execute(
                "UPDATE Hospital.Registration SET Patient_Id = @Patient_Id, RegistrationDate = @RegistrationDate, FirstName = @FirstName, LastName = @LastName " +
                "WHERE Registration_Id = @Registration_Id", registration);
            if (rowAffected == 0)
            {
                return BadRequest(new { message = "Update failed. Registration not found." });
            }
            return Ok(registration);
        }
        //Delete operation
        //Delete api/Registration/id
        [HttpDelete("{id}")]
        public ActionResult DeleteRegistration(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rawAffected = connection.Execute("DELETE FROM Hospital.Registration WHERE Registration_Id = @Id", new { Id = @id });
            if (rawAffected == 0)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}

