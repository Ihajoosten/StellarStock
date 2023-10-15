namespace StellarStock.Application.ExceptionHandling
{
    public class QueryExecutionException : Exception
    {
        public QueryExecutionException(string message) : base(message) { }
    }
}
