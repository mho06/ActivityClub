using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActivityClub.Models
{
    public class AdminViewModel
    {
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public IEnumerable<User> Admins { get; set; } = new List<User>();
        [Required]
        public IEnumerable<Guide> Guides { get; set; } = new List<Guide>();
        [Required]
        public IEnumerable<Event> Events { get; set; } = new List<Event>();
        [Required]
        public IEnumerable<Member> Members { get; set; } = new List<Member>();
        [Required]
        public IEnumerable<Lookup> Lookups { get; set; } = new List<Lookup>();
    }
}
