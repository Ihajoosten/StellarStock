using Microsoft.EntityFrameworkCore;
using StellarStock.Domain.Entities.Base;
using StellarStock.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Supplier Name cannot exceed 25 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(25, ErrorMessage = "Supplier Phone cannot exceed 25 characters")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Contact Email is required.")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(35, ErrorMessage = "Supplier Contact Email cannot exceed 35 characters")]
        public required string ContactEmail { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public required AddressVO Address { get; set; }

        [Required(ErrorMessage = "Active Status is required.")]
        public required bool IsActive { get; set; }

        [Required(ErrorMessage = "Validity Period is required.")]
        public required DateRangeVO ValidityPeriod { get; set; }

        public ICollection<InventoryItem>? SuppliedItems { get; set; }
    }
}