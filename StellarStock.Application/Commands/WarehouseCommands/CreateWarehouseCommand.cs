using StellarStock.Domain.ValueObjects;

namespace StellarStock.Application.Commands.WarehouseCommands
{
    public class CreateWarehouseCommand
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public AddressVO Address { get; set; }
        public bool IsOpen { get; set; }
    }
}
