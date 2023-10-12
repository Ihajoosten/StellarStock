﻿using StellarStock.Domain.Entities.Base;
using StellarStock.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellarStock.Domain.Entities
{
    public class InventoryItem : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [MaxLength(75, ErrorMessage = "Name cannot exceed 75 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [DataType(DataType.MultilineText)]
        [MaxLength(150, ErrorMessage = "Description cannot exceed 150 characters")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public required ItemCategory Category { get; set; }

        [Required(ErrorMessage = "Popularity Score is required.")]
        public required int PopularityScore { get; set; }

        [Required(ErrorMessage = "Product Code is required.")]
        public required ProductCodeVO ProductCode { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public required QuantityVO Quantity { get; set; }

        [Required(ErrorMessage = "Money is required.")]
        public required MoneyVO Money { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "Location ID is required.")]
        public required string LocationId { get; set; }

        [Required(ErrorMessage = "Validity Period is required.")]
        public required DateRangeVO ValidityPeriod { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        public required string SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }
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
