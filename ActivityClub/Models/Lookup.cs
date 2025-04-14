using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActivityClub.Models
{
    public class Lookup
    {
        public int LookupID { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string LookupName { get; set; }

        public int Order { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
