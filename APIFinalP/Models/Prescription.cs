namespace APIFinalP.Models
{
    public class Prescription
    {
        private string dosage;

        public int Prescription_Id { get; set; }
        public int Patient_Id { get; set; }
        public int Doctor_Id { get; set; }
        public string Medication { get; set; }
        public string Dosage { get => dosage; set => dosage = value; }

    }
}
