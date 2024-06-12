namespace APIFinalP.Models
{
    public class Admission
    {
        public int Admission_Id { get; set; }
        public int Patient_Id { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime DischargeDate { get; set; }
    }
}

