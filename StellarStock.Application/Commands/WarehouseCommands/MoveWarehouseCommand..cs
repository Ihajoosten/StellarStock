namespace StellarStock.Application.Commands.WarehouseCommands
{
    public class MoveWarehouseCommand
    {
        public string WarehouseId { get; set; }
        public string NewAddress { get; set; }
        public string NewCity { get; set; }
        public string NewRegion { get; set; }
        public string NewCountry { get; set; }
        public string NewPostalCode { get; set; }
    }
}
