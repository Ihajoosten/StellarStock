namespace StellarStock.Application.Commands.Concrete.SupplierCommands
{
    public class UpdateSupplierCommand : ISupplierCommand
    {
        public string Id { get; set; }
        public string NewName { get; set; }
        public string NewPhone { get; set; }
        public string NewContactEmail { get; set; }
        public AddressVO NewAddress { get; set; }
    }
}