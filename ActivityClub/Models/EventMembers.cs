using ActivityClub.Models;

public class EventMembers
{
    public int EventID { get; set; }
    public int MemberID { get; set; }

    public Event Event { get; set; } = null!;
    public User Member { get; set; } = null!;
}
