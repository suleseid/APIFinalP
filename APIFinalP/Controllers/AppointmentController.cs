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
        //Inside the ActionResult there is a generic, which is acual shape of the data.
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
            // Check if the patient and doctor exist (optional but recommended for data integrity)
            if (appointment.Patient_Id < 1 || appointment.Doctor_Id < 1)
            {
                return BadRequest(new { Message = "Invalid Patient_Id or Doctor_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = appointment.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { Message = "Patient Not Found." });
            }

            // Check if the Doctor exists
            Doctor doctor = connection.QueryFirstOrDefault<Doctor>(
                "SELECT * FROM Hospital.Doctor WHERE Doctor_Id = @Id", new { Id = appointment.Doctor_Id });

            if (doctor == null)
            {
                return BadRequest(new { message = "Doctor Not Found." });
            }

            try
            {
                Appointment newAppointment = connection.QuerySingle<Appointment>(
                    "INSERT INTO Hospital.Appointment (Patient_Id, Doctor_Id, AppointmentDate, RegistrationDate) " +
                    "OUTPUT INSERTED.* VALUES (@Patient_Id, @Doctor_Id, @AppointmentDate, @RegistrationDate);", appointment);

                return Ok(newAppointment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { message = "An error occurred while creating the appointment.", error = ex.Message });
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
                "UPDATE Hospital.Appointment SET Patient_Id =@Patient_Id, Doctor_Id =@Doctor_Id, AppointmentDate =@AppointmentDate, RegistrationDate =@RegistrationDate" +
                " WHERE Appointment_Id =@Appointment_Id ", appointment);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(appointment);
        }
        //Delete operation
        //Delete api/Appointment/id
        [HttpDelete("{id}")]

        public ActionResult DeleteAppointment(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rowAffected = connection.Execute("DELETE FROM Hospital.Appointment WHERE appointment_Id = @Id", new { Id = id });

            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
