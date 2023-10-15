namespace StellarStock.Test.Infrastructure
{
    public class GenericRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        public GenericRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEntities()
        {
            // clean up database
            _fixture.ClearData<InventoryItem>();

            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            // Add test data to the in-memory database
            var dummyItem = new InventoryItem()
            {
                Id = "dummy-1",
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            context.InventoryItems.Add(dummyItem);
            context.SaveChanges();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // clean up database
            _fixture.ClearData<InventoryItem>();

            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            // Add test data to the in-memory database
            var dummyItem = new InventoryItem()
            {
                Id = "dummy-1",
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            context.InventoryItems.Add(dummyItem);
            context.SaveChanges();

            // Act
            var result = await repository.GetByIdAsync(dummyItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<InventoryItem>(result);
            Assert.Equal(dummyItem.Id, result.Id);
            Assert.Equal(dummyItem.Category, result.Category);
            Assert.Equal(dummyItem.ProductCode.Code, result.ProductCode.Code);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // clean up database
            _fixture.ClearData<InventoryItem>();

            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);


            // Add an entity to update
            var dummyItem = new InventoryItem()
            {
                Id = "dummy-1",
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await repository.AddAsync(dummyItem);

            // Act
            // Modify properties of the entity
            dummyItem.Name = "UpdatedValue";
            var result = await repository.UpdateAsync(dummyItem);

            // Assert
            Assert.True(result);
            // Check if the entity is updated in the in-memory database
            var updatedEntity = await repository.GetByIdAsync(GetEntityId(dummyItem));
            Assert.NotNull(updatedEntity);
            Assert.Equal("UpdatedValue", updatedEntity.Name);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveEntity()
        {
            // clean up database
            _fixture.ClearData<InventoryItem>();

            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            // Add an entity to update
            var dummyItem = new InventoryItem()
            {
                Id = "dummy-1",
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await repository.AddAsync(dummyItem);

            // Act
            var result = await repository.RemoveAsync(GetEntityId(dummyItem));

            // Assert
            Assert.True(result);
            // Check if the entity is removed from the in-memory database
            var removedEntity = await repository.GetByIdAsync(GetEntityId(dummyItem));
            Assert.Null(removedEntity);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistentId_ShouldReturnNull()
        {
            // clean up database
            _fixture.ClearData<InventoryItem>();

            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            // Act
            var result = await repository.GetByIdAsync("NonExistentId");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_NonExistentId_ShouldReturnFalse()
        {
            // clean up database
            _fixture.ClearData<InventoryItem>();

            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            // Act
            var result = await repository.RemoveAsync("NonExistentId");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddAsync_EntityWithNullId_ShouldReturnFalse()
        {
            // clean up database
            _fixture.ClearData<InventoryItem>();

            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            // Act
            var entityWithNullId = new InventoryItem()
            {
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var result = await repository.AddAsync(entityWithNullId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddAsync_ExceptionDuringAdd_ShouldRollbackTransaction()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>();
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork.Object, logger.Object);

            var entity = new InventoryItem()
            {
                Id = "dummy-1",
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Set up the unitOfWork to throw an exception during Add
            unitOfWork.Setup(u => u.BeginTransactionAsync()).ThrowsAsync(new Exception("Simulated exception during Add"));

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => repository.AddAsync(entity));

            // Verify that RollbackAsync was called
            unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExceptionDuringAdd_ShouldRollbackTransaction()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>();
            var repository = new EFGenericRepository<InventoryItem>(unitOfWork.Object, logger.Object);

            var entity = new InventoryItem()
            {
                Id = "dummy-1",
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Set up the unitOfWork to throw an exception during Add
            unitOfWork.Setup(u => u.BeginTransactionAsync()).ThrowsAsync(new Exception("Simulated exception during Add"));

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => repository.UpdateAsync(entity));

            // Verify that RollbackAsync was called
            unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        }

        private string GetEntityId(InventoryItem entity)
        {
            // Implement this method to extract the entity ID
            return entity.Id.ToString();
        }
    }
}
