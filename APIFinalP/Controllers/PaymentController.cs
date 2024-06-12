using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using APIFinalP.Models;
using System.Globalization;

namespace APIFinalP.Controllers
{
    //This is the route to the controller
    [Route("api/[controller]")]
    [ApiController]
    //We inherit from ControllerBase
    public class PaymentController : ControllerBase
    {
        //GET: api/Payment
        //This endpoint is typically returns all of the entities (payment) from the database
        //Select * from Payment
        //Constractor will initialize properly
        string connectionString;

        public PaymentController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("APIFinalP");
            //Console.WriteLine(connectionString);
        }
        //get
        //api/payments
        [HttpGet]
        public ActionResult<List<Payment>> GetAllPayments()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            List<Payment> payments = connection.Query<Payment>("Select * From Hospital.Payment").ToList();
            return Ok(payments);
        }
        //Get with an id
        //Api/payment/id
        [HttpGet("{id}")]
        public ActionResult<Payment> GetPayments(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            // Using parameterized queries to prevent SQL injection attacks
            Payment payments = connection.QueryFirstOrDefault<Payment>(
                "SELECT * From Hospital.Payment WHERE Payment_Id = @Id", new { Id = id });
            //Check to see that we got a payment by Id
            if (payments == null)
            {
                //if no payment return a 404
                return NotFound();
            }
            //return payments
            return Ok(payments);
        }


        //Post-Create
        //Put-Update
        //Delete
        [HttpPost]
        public ActionResult<Payment> CreateAdmission(Payment payment)
        {
            if (payment.Patient_Id < 1)
            {
                return BadRequest(new { message = "Invalid Patient_Id." });
            }
            using SqlConnection connection = new SqlConnection(connectionString);

            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = payment.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }

            try
            {
                Payment newPayment = connection.QuerySingle<Payment>(
                    "INSERT INTO Hospital.Payment (Patient_Id, Amount, PaymentDate) " +
                    "OUTPUT INSERTED.* VALUES (@Patient_Id, @Amount, @PaymentDate);", payment);

                return Ok(newPayment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = "An error occurred while creating the payment.", error = ex.Message });
            }
        }

        //Update operation
        //PUT api/payment/id
        [HttpPut("{id}")]
        public ActionResult<Payment> UpdatePayment(int id, Payment payment)
        {
            if (id < 1)
            {
                return BadRequest(new { message = "Invalid Payment_Id." });
            }

            // Declare and initialize the paymentDateString variable
            string paymentDateString = payment.PaymentDate.ToString("yyyy-MM-ddTHH:mm:ss");

            // Use the paymentDateString variable for parsing
            if (!DateTime.TryParseExact(paymentDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest(new { message = "Invalid PaymentDate format. Please use 'yyyy-MM-ddTHH:mm:ss' format." });
            }

            payment.Payment_Id = id;
            using SqlConnection connection = new SqlConnection(connectionString);

            // Log the Patient_Id being checked
            Console.WriteLine($"Checking for Patient_Id: {payment.Patient_Id}");


            // Check if the patient exists
            Patient patient = connection.QueryFirstOrDefault<Patient>(
                "SELECT * FROM Hospital.Patient WHERE Patient_Id = @Id", new { Id = payment.Patient_Id });

            if (patient == null)
            {
                return BadRequest(new { message = "Patient not found." });
            }
            int rowAffected = connection.Execute(
                "UPDATE Hospital.Payment SET Patient_Id = @Patient_Id, Amount = @Amount, PaymentDate = @PaymentDate " +
                "WHERE Payment_Id = @Payment_Id", payment);
            if (rowAffected == 0)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        //Delete operation
        //Delete api/Payment/id
        [HttpDelete("{id}")]
        public ActionResult DeletePayment(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            using SqlConnection connection = new SqlConnection(connectionString);
            int rawAffected = connection.Execute("DELETE FROM Hospital.Payment WHERE Payment_Id = @Id", new { Id = @id });
            if (rawAffected == 0)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
