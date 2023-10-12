using StellarStock.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Supplier Name cannot exceed 25 characters")]
        public required string SupplierName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Supplier Address cannot exceed 25 characters")]
        public required string Address { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [MaxLength(10, ErrorMessage = "Supplier Postal Code cannot exceed 10 characters")]
        public required string PostalCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Supplier City cannot exceed 25 characters")]
        public required string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Supplier Region cannot exceed 25 characters")]
        public required string Region { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Supplier Country cannot exceed 25 characters")]
        public required string Country { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(25, ErrorMessage = "Supplier Phone cannot exceed 25 characters")]
        public required string Phone { get; set; }
    }
}
