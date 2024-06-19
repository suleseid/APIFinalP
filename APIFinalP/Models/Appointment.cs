namespace APIFinalP.Models
{
    public class Appointment
    {
        public int Appointment_Id { get; set; }
        public int Patient_Id { get; set; }
        public int Doctor_Id { get; set; }
        public string AppointmentDate { get; set; }
        public string RegistrationDate { get; set; }
    }
}
