using StellarStock.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Entities
{
    public class Location : BaseEntity
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Store Name cannot exceed 25 characters")]
        public required string StoreName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Store Address cannot exceed 25 characters")]
        public required string Address { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [MaxLength(10, ErrorMessage = "Store Postal Code cannot exceed 10 characters")]
        public required string PostalCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Store City cannot exceed 25 characters")]
        public required string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Store Region cannot exceed 25 characters")]
        public required string Region { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Store Country cannot exceed 25 characters")]
        public required string Country { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(25, ErrorMessage = "Store Phone cannot exceed 25 characters")]
        public required string Phone { get; set; }
    }
}

