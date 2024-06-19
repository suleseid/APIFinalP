namespace APIFinalP.Models
{
    public class Registration
    {
        public int Registration_Id { get; set; }
        public int Patient_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
