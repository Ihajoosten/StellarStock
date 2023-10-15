namespace StellarStock.Test.Config
{
    public class DatabaseFixture : IDisposable
    {
        public TestDbContext Context { get; private set; }

        public DatabaseFixture()
        {
            // Initialize your context or other setup here
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            Context = new TestDbContext(options);
        }

        public void Dispose()
        {
            // Clean up resources, e.g., close the context
            Context.Dispose();
        }

        public void ClearData<T>() where T : class
        {
            var tableData = Context.Set<T>();
            tableData.RemoveRange(tableData);
            Context.SaveChanges();
        }
    }
}
