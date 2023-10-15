namespace StellarStock.Application.Dto
{
    internal class WarehouseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public AddressVO Address { get; set; }
        public bool IsOpen { get; set; }
        public ICollection<InventoryItem>? StockedItems { get; set; }
    }
}