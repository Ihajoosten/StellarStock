namespace StellarStock.Application.Commands.WarehouseCommands
{
    public class ReopenWarehouseCommand : ICommand
    {
        public string WarehouseId { get; set; }
    }
}
