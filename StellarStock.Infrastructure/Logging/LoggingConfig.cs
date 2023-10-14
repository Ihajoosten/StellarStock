using Serilog;

namespace StellarStock.Infrastructure.Logging
{
    public class LoggingConfig
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
