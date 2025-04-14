namespace ActivityClub.Models
{
    public class Member
    {
        public int MemberID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MemberPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }  
        public string Gender { get; set; }
        public DateTime? JoiningDate { get; set; }  
        public string MobileNumber { get; set; }
        public string EmergencyNumber { get; set; }
        public byte[] Photo { get; set; }          
        public string Profession { get; set; }
        public string Nationality { get; set; }
    }
}
