namespace StellarStock.Application.ExceptionHandling
{
    public class CommandExecutionException : Exception
    {
        public CommandExecutionException(string message) : base(message) { }
    }
}
