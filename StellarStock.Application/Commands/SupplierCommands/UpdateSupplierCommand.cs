using StellarStock.Domain.ValueObjects;

namespace StellarStock.Application.Commands.SupplierCommands
{
    public class UpdateSupplierCommand
    {
        public string SupplierId { get; set; }
        public string NewName { get; set; }
        public string NewPhone { get; set; }
        public string NewContactEmail { get; set; }
        public AddressVO NewAddress { get; set; }
    }
}
