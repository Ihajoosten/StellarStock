using StellarStock.Application.Commands.SupplierCommands;

namespace StellarStock.Test.Application
{
    public class SupplierQueryHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        public SupplierQueryHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
