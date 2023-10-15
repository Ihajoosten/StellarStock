namespace StellarStock.Application.Commands.Concrete.WarehouseCommands
{
    public class CreateWarehouseCommand : IWarehouseCommand
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public AddressVO Address { get; set; }
        public bool IsOpen { get; set; }
    }
}