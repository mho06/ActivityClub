using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityClub.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public char? Gender { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W)(?=.*\d)(?=.*[a-z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one capital letter, one special character, one number, and one lowercase letter")]
        public string Pass { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Pass", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string UserRole { get; set; }

        public ICollection<EventGuides>? EventGuides { get; set; }

        public ICollection<EventMembers>? EventMembers { get; set; } = new List<EventMembers>();
    }
}
