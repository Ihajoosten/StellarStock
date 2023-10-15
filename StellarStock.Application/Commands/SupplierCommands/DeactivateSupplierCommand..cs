namespace StellarStock.Application.Commands.SupplierCommands
{
    public class DeactivateSupplierCommand : ICommand
    {
        public string SupplierId { get; set; }
    }
}
