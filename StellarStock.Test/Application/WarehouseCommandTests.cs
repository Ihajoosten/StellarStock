namespace StellarStock.Test.Application
{
    public class WarehouseCommandTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public WarehouseCommandTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            // clean up database
            _fixture.ClearData<Warehouse>();
        }
    }
}
