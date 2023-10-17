namespace StellarStock.Application.Dto
{
    public class SupplierDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ContactEmail { get; set; }
        public AddressVO Address { get; set; }
        public bool IsActive { get; set; }
        public DateRangeVO ValidityPeriod { get; set; }
        public ICollection<InventoryItem>? SuppliedItems { get; set; }
    }
}