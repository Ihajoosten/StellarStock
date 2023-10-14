namespace StellarStock.Domain.Entities
{
    public class Warehouse : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Store Name cannot exceed 25 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(25, ErrorMessage = "Store Phone cannot exceed 25 characters")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public required AddressVO Address { get; set; }

        [Required(ErrorMessage = "Open Status is required.")]
        public required bool IsOpen { get; set; }

        public ICollection<InventoryItem>? StockedItems { get; set; }
    }
}

