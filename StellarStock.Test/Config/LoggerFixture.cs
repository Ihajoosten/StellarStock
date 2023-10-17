namespace StellarStock.Test.Config
{
    public class LoggerFixture
    {
        public static ILogger<T> CreateLogger<T>()
        {
            // In a real application, you might set up your logging configuration here.
            // For simplicity, we'll use the console logger in this example.
            var factory = LoggerFactory.Create(builder => builder.AddConsole());
            return factory.CreateLogger<T>();
        }
    }
}
