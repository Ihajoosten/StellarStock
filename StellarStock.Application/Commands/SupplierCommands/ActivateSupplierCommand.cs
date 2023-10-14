namespace StellarStock.Application.Commands.SupplierCommands
{
    public class ActivateSupplierCommand : ICommand
    {
        public string SupplierId { get; set; }
    }
}
