
namespace APIFinalP.Models
{
    //Models are used just a C# representation of a table in the database.
    public class Doctor
    {
        //When we constract properties we can use the shortway
        //the short way we can use prop tap tap
        public int Doctor_Id { get; set; }//This is used to identify each doctor in the system.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }//Used to categorize and identify the doctor's specialty.
        public int Department_Id { get; set; }//Used to link the doctor to a specific department within the hospital.
        public int Patient_Id { get; set; }//This property used a patient associated with the doctor.
       //And used to link the doctor to a specific patient they are treating.
    }
}
