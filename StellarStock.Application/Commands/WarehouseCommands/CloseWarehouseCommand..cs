namespace StellarStock.Application.Commands.WarehouseCommands
{
    public class CloseWarehouseCommand : ICommand
    {
        public string WarehouseId { get; set; }
    }
}
