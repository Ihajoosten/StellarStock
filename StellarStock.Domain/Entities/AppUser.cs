using Microsoft.AspNetCore.Identity;
using StellarStock.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(50, ErrorMessage = "Full name cannot exceed 50 characters")]
        public required string FullName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Birth Date is required.")]
        [DateRange("1900-01-01", ErrorMessage = "Invalid birth date.")]
        public required DateTime BirthDate { get; set; }
    }
}
