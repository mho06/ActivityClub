

namespace ActivityClub.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string EventDes { get; set; }
        public string Destination { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Cost { get; set; }
        public string Stat { get; set; }
        public int CategoryLookupID { get; set; }
        public Lookup? CategoryLookup { get; set; }
        public int LookupID { get; set; }

        public ICollection<EventGuides> EventGuides { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
