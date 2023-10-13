using StellarStock.Domain.Entities.Base;
using StellarStock.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace StellarStock.Domain.Entities
{
    public class Location : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Store Name cannot exceed 25 characters")]
        public required string StoreName { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(25, ErrorMessage = "Store Phone cannot exceed 25 characters")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public required AddressVO LocationAddress { get; set; }

        [Required(ErrorMessage = "Open Status is required.")]
        public required bool IsOpen { get; set; }

        public ICollection<InventoryItem>? InventoryItems { get; set; }
    }
}

