using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActivityClub.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        [Required]
        public string EventDes { get; set; }
        [Required]
        public string Destination { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Cost { get; set; }
        [Required]
        public string Stat { get; set; }
        public int? CategoryLookupID { get; set; }  
        public Lookup? CategoryLookup { get; set; }
        public int? LookupID { get; set; }  

        public ICollection<EventMembers>? EventMembers { get; set; } = new List<EventMembers>();
        public ICollection<EventGuides>? EventGuides { get; set; } = new List<EventGuides>();
    }
}
