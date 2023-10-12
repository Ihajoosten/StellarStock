using StellarStock.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellarStock.Domain.Entities
{
    public class InventoryItem : BaseEntity
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(75, ErrorMessage = "Name cannot exceed 75 characters")]
        public required string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(150, ErrorMessage = "Description cannot exceed 150 characters")]
        public required string Description { get; set; }

        [Required]
        public required ItemCategory Category { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public required int Quantity { get; set; }

        // Navigation properties
        [Required]
        public required string LocationId { get; set; }

        [ForeignKey("LocationId")]
        public required Location? Location { get; set; }

        [Required]
        public required string SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public required Supplier Supplier { get; set; }
    }

    public enum ItemCategory
    {
        Laptop,
        Smartphone,
        Tablet,
        DesktopComputer,
        SmartTV,
        Camera,
        AudioDevice,
        Navigation,
        Kitchen,
        Wearable,
        Accessories,
    }
}
