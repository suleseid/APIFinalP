namespace APIFinalP.Models
{
    public class Medicalrecord
    {
        public int Medicalrecord_Id { get; set; }
        public int Patient_Id { get; set; }
        public int Doctor_Id { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }
}
