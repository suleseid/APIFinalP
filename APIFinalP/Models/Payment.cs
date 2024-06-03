

namespace APIFinalP.Models
{
    public class Payment
    {
        public int Payment_Id { get; set; }
        public int Patient_Id { get; set; }
        public decimal Amount { get; set; } // Changed from string to decimal for currency handling     
        public DateTime PaymentDate {  get; set; }
        
    }
}
