namespace APIFinalP.Models
{
    //Models are used just a C# representation of a table in a database.
    public class Patient
    {
        public int Patient_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
    }
}
