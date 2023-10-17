namespace StellarStock.Application.Commands.Concrete.WarehouseCommands
{
    public class UpdateWarehouseCommand : IWarehouseCommand
    {
        public string Id { get; set; }
        public string NewName { get; set; }
        public string NewPhone { get; set; }
        public AddressVO NewAddress { get; set; }
    }
}