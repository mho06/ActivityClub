using ActivityClub.Models;

public class EventGuides
{
    public int EventID { get; set; }
    public int GuideID { get; set; }
    public int? UserID { get; set; } 

    public Event Event { get; set; } = null!;
    public Guide Guide { get; set; } = null!;
    public User? User { get; set; }
}
