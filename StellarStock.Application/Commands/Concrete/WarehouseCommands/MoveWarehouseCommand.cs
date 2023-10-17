namespace StellarStock.Application.Commands.Concrete.WarehouseCommands
{
    public class MoveWarehouseCommand : IWarehouseCommand
    {
        public string Id { get; set; }
        public string NewAddress { get; set; }
        public string NewCity { get; set; }
        public string NewRegion { get; set; }
        public string NewCountry { get; set; }
        public string NewPostalCode { get; set; }
    }
}