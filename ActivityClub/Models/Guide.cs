

namespace ActivityClub.Models
{
    public class Guide
    {
        public int GuideID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } = "Guide@123321";
        public DateTime? DateOfBirth { get; set; } 
        public DateTime? JoiningDate { get; set; } 
        public byte[]? Photo { get; set; } 
        public string? Profession { get; set; }

        public virtual ICollection<EventGuides> EventGuides { get; set; } = new List<EventGuides>();
    }

}
