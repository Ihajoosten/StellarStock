namespace StellarStock.Application.Commands.Concrete.SupplierCommands
{
    public class DeleteSupplierCommand : ISupplierCommand
    {
        public string Id { get; set; }
    }
}