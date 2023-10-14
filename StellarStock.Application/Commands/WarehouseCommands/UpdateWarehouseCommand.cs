using StellarStock.Domain.ValueObjects;

namespace StellarStock.Application.Commands.WarehouseCommands
{
    public class UpdateWarehouseCommand
    {
        public string WarehouseId { get; set; }
        public string NewName { get; set; }
        public string NewPhone { get; set; }
        public AddressVO NewAddress { get; set; }
    }
}
