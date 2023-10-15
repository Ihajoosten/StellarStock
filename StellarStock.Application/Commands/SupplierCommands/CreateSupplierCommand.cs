namespace StellarStock.Application.Commands.SupplierCommands
{
    public class CreateSupplierCommand : ICommand
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ContactEmail { get; set; }
        public AddressVO Address { get; set; }
        public DateRangeVO ValidityPeriod { get; set; }
    }
}
