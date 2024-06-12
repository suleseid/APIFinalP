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
    public class AppointmentController : ControllerBase
    {
        //GET: api/Appointment
        //This endpoint is typically returns all of the entities (appointment) from the database
        //Select * from Appointment
        //Constractor will initialize properly
        string connectionString;

        public AppointmentController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/appointment
        [HttpGet]
        public ActionResult<List<Appointment>> GetAllAppointments()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Appointment> appointments = connection.Query<Appointment>("Select * From Hospital.Appointment").ToList();
            return Ok(appointments);
        }
        //Get with an id
        //Api/Appointment/id
        [HttpGet("{id}")]
        public ActionResult<Appointment> GetAppointments(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Appointment appointments = connection.QueryFirstOrDefault<Appointment>(
                "SELECT * From Hospital.Appointment WHERE Appointment_Id = @Id", new { Id = id });
            //Check to see that we got a appointment by Id
            if (appointments == null)
            {
                //if no appointment return a 404
                return NotFound();
            }
            //return Appointments
            return Ok(appointments);
        }

        //Post-Create
        //Put-Update
        //Delete

        [HttpPost]
        public ActionResult<Appointment> CreateAppointment(Appointment appointment)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(appointment);

            try
            {
                Appointment newAppointment = connection.QuerySingle<Appointment>(
                    "INSERT INTO Hospital.Appointment(Patient_Id, Doctor_Id, Nurse_Id, AppointmentDate, RegistrationDate) " +
                    "VALUES (@Patient_Id, @Doctor_Id, @Nurse_Id, @AppointmentDate, @RegistrationDate); SELLECT * FROM Hospital.Appointment " +
                    "WHERE Appointment_Id = SCOPE_IDENTITY(); ", appointment);
                return Ok(newAppointment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }
        //PUT api/Appointment/id
        [HttpPut("{id}")]
        public ActionResult<Appointment> UpdateAppointment(int id, Appointment appointment)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            appointment.Appointment_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(appointment);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Appointment SET Patient_Id =@Patient_Id, Doctor_Id =@Doctor_Id, Nurse_Id =@Nurse_Id, AppointmentDate =@AppointmentDate, RegistrationDate =@RegistrationDate" +
                " WHERE Appointment_Id =@Appointment_Id ", appointment);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(appointment);
        }
    }
}
