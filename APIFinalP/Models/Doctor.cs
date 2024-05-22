
namespace APIFinalP.Models
{
    //Models are used just a C# representation of a table in a database.
    public class Doctor
    {
        //When we constract properties we can use the shortway
        //the short way we can use prop tap tap
        public int Doctor_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public int Department_Id { get; set; }
        public int Patient_Id { get; set; }
    }
}
