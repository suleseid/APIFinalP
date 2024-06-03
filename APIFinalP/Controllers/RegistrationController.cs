using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using APIFinalP.Models;
using System.Collections;

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
            //Console.WriteLine(connectionString);
        }
        //get
        //api/registrations
        [HttpGet]
        public ActionResult<List<Registration>> GetAllRegistrations()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Registration> registrations = connection.Query<Registration>("Select * From Hospital.Registration").ToList();
            return Ok(registrations);
        }
        //Get with an id
        //Api/Registration/id
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
            return Ok(registration);

            try
            {
                Registration newRegistration = connection.QuerySingle<Registration>(
                    "INSERT INTO Hospital.Registration(First Name, Last Name, Specialization,Department_Id, Patient_Id) " +
                    "VALUES (@First Name, @Last Name, @Specialization, @Department_Id, @Patient_Id); SELLECT * FROM Hospital.Registration " +
                    "WHERE Registration_Id = SCOPE_IDENTITY(); ", registration);
                return Ok(newRegistration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }
        //PUT api/Registration/id
        [HttpPut("{id}")]
        public ActionResult<Registration> UpdateRegistration(int id, Registration registration)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            registration.Registration_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(registration);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Registration SET First Name =@First Name, Last Name =@Last Name, Specialization = @Specialization, Department_Id =@Department_Id, Patient_Id =@Patient_Id " +
                " WHERE Registration_Id =@Doctor_Id ", registration);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(registration);
        }
    }
}
