namespace StellarStock.Application.ExceptionHandling
{
    public class CommandValidationException : Exception
    {
        public CommandValidationException(string message) : base(message) { }
    }
}
