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
    public class PrescriptionController : ControllerBase
    {
        //GET: api/Prescription
        //This endpoint is typically returns all of the entities (prescription) from the database
        //Select * from Prescription
        //Constractor will initialize properly
        string connectionString;

        public PrescriptionController (IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/prescription
        [HttpGet]
        public ActionResult<List<Prescription>> GetAllPrescriptions()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Prescription> prescriptions = connection.Query<Prescription>("Select * From Hospital.Prescription").ToList();
            return Ok(prescriptions);
        }
        //Get with an id
        //Api/prescription/id
        [HttpGet("{id}")]
        public ActionResult<Prescription> GetPrescripton(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Prescription prescriptions = connection.QueryFirstOrDefault<Prescription>(
               "SELECT * From Hospital.Prescription WHERE Prescription_Id = @Id", new { Id = id });
            //Check to see that we got a prescription by Id
            if (prescriptions == null)
            {
                //if no  prescription return a 404
                return NotFound();
            }
            //return Appointments
            return Ok(prescriptions);
        }

        //Post-Create
        //Put-Update
        //Delete
        [HttpPost]
        public ActionResult<Prescription> CreatePrescriptions (Prescription prescription)
        {
            if (prescription.Patient_Id < 1 || prescription.Doctor_Id < 1)
            {
                return BadRequest(new { message = "Invalid Patient_Id or Doctor_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = prescription.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }

            // Check if the doctor exists
            Doctor doctor = connection.QueryFirstOrDefault<Doctor>(
                "SELECT * FROM Hospital.Doctor WHERE Doctor_Id = @Id", new { Id = prescription.Doctor_Id });

            if (doctor == null)
            {
                return BadRequest(new { message = "Doctor not found." });
            }

            try
            {
                Prescription newPrescription = connection.QuerySingle<Prescription>(
                    "INSERT INTO Hospital.Prescription (Patient_Id, Doctor_Id, Medication, Dosage) " +
                    "OUTPUT INSERTED.* VALUES (@Patient_Id, @Doctor_Id, @Medication, @Dosage;", prescription);

                return Ok(newPrescription);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = "An error occurred while creating the prescription.", error = ex.Message });
            }
        }


        //Update opreations
        //PUT api//id
        [HttpPut("{id}")]
        public ActionResult<Prescription> UpdatePrescription(int id, Prescription prescription)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            prescription.Prescription_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);
            //return Ok(Prescription);
            //PUT- we have to send every information wheether changed or not
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Prescription SET Patient_Id =@Patient_Id, Doctor_Id =@Doctor_Id, Medication = @Medication, Dosage =@Dosage " +
                "WHERE Prescription_Id =@Prescription_Id ", prescription);
            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok(prescription);
        }
        //Delete operation
        //Delete api/Prescription/id
        [HttpDelete("{id}")]

        public ActionResult DeletePrescription(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rowAffected = connection.Execute("DELETE FROM Hospital.Prescription WHERE Prescription_Id = @Id", new { Id = id });

            if (rowAffected == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
