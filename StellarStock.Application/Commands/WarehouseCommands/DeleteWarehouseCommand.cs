namespace StellarStock.Application.Commands.WarehouseCommands
{
    public class DeleteWarehouseCommand : ICommand
    {
        public string WarehouseId { get; set; }
    }
}
