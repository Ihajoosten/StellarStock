using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Full name cannot exceed 50 characters")]
        public required string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public required DateOnly BirthDate { get; set; }
    }
}
