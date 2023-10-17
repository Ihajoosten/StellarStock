using StellarStock.Application.Commands.Concrete.WarehouseCommands;

namespace StellarStock.Test.Application
{
    public class WarehouseCommandHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public WarehouseCommandHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            // clean up database
            _fixture.ClearData<InventoryItem>();
            _fixture.ClearData<Supplier>();
            _fixture.ClearData<Warehouse>();
        }

        [Fact]
        public async Task HandleAsync_CreateWarehouseCommand_CallsHandleCreateAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                // Arrange
                var command = new CreateWarehouseCommand
                {
                    Name = "TestName",
                    Phone = "12345678910",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    IsOpen = true,
                };

                var handlerMock = new Mock<IWarehouseCommandHandler<CreateWarehouseCommand>>();
                var repoMock = new Mock<IGenericRepository<Warehouse>>();
                var handler = new WarehouseCommandHandler<CreateWarehouseCommand>(
                    repoMock.Object,
                    Mock.Of<ILogger<WarehouseCommandHandler<CreateWarehouseCommand>>>());

                // Act
                await handler.HandleAsync(command);

                // Assert
                repoMock.Verify(repo => repo.AddAsync(It.IsAny<Warehouse>()), Times.Once);

                // If everything is fine, commit the transaction
                transaction.Commit();
            }
            finally
            {
                // Roll back the transaction to undo any changes made during the test
                transaction.Rollback();
            }
        }

        [Fact]
        public async Task HandleAsync_UpdateWarehouseCommand_CallsHandleUpdateAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Warehouse>>>().Object;
                var genericRepository = new EFGenericRepository<Warehouse>(unitOfWork, logger);

                var guid = Guid.NewGuid().ToString();
                var warehouse = new Warehouse
                {
                    Id = guid,
                    Name = "TestName",
                    Phone = "12345678910",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    IsOpen = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await genericRepository.AddAsync(warehouse);

                var x = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(x);
                Assert.True(x.Id == guid);

                var command = new UpdateWarehouseCommand
                {
                    Id = guid,
                    NewName = "UpdatedWarehouse",
                    NewAddress = new AddressVO("newStreet", "newPostalCode", "testCity", "testRegion", "testCountry"),
                    NewPhone = "9876543210"
                };

                var handler = new WarehouseCommandHandler<UpdateWarehouseCommand>(
                    genericRepository,
                    Mock.Of<ILogger<WarehouseCommandHandler<UpdateWarehouseCommand>>>());

                // Act
                var result = await handler.HandleAsync(command);
                var value = result.Values.First();

                // Assert
                Assert.True(value);
                Assert.True(result.Keys.Count == 1);
                Assert.True(result.Values.Count == 1);
                Assert.Equal(guid, result.Keys.First());

                // Assert
                var updated = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(updated);
                Assert.Equal("UpdatedWarehouse", updated.Name);
                Assert.Equal("9876543210", updated.Phone);
                Assert.Equal("newStreet", updated.Address.Street);
                Assert.Equal("newPostalCode", updated.Address.PostalCode);

                // If everything is fine, commit the transaction
                transaction.Commit();
            }
            finally
            {
                // Roll back the transaction to undo any changes made during the test
                transaction.Rollback();
            }
        }

        [Fact]
        public async Task HandleAsync_DeleteWarehouseCommand_CallsHandleDeleteAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Warehouse>>>().Object;
                var genericRepository = new EFGenericRepository<Warehouse>(unitOfWork, logger);

                var handler = new WarehouseCommandHandler<DeleteWarehouseCommand>(
                    genericRepository,
                    Mock.Of<ILogger<WarehouseCommandHandler<DeleteWarehouseCommand>>>());

                var guid = Guid.NewGuid().ToString();
                var warehouse = new Warehouse
                {
                    Id = guid,
                    Name = "TestName",
                    Phone = "12345678910",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    IsOpen = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    StockedItems = new List<InventoryItem>()
                };

                await genericRepository.AddAsync(warehouse);

                var x = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(x);
                Assert.True(x.Id == guid);

                var command = new DeleteWarehouseCommand
                {
                    Id = guid,
                };

                // Act
                var result = await handler.HandleAsync(command);
                var value = result.Values.First();

                // Assert
                Assert.True(value);
                Assert.True(result.Keys.Count == 1);
                Assert.True(result.Values.Count == 1);
                Assert.Equal(guid, result.Keys.First());

                // Ensure the item is deleted from the repository
                var deleted = await genericRepository.GetByIdAsync(guid);
                Assert.Null(deleted);

                // If everything is fine, commit the transaction
                transaction.Commit();
            }
            finally
            {
                // Roll back the transaction to undo any changes made during the test
                transaction.Rollback();
            }
        }

        [Fact]
        public async Task HandleAsync_MoveWarehouseCommand_CallsHandleMoveAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Warehouse>>>().Object;
                var genericRepository = new EFGenericRepository<Warehouse>(unitOfWork, logger);

                var guid = Guid.NewGuid().ToString();
                var warehouse = new Warehouse
                {
                    Id = guid,
                    Name = "TestName",
                    Phone = "12345678910",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    IsOpen = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await genericRepository.AddAsync(warehouse);

                var x = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(x);
                Assert.True(x.Id == guid);

                var command = new MoveWarehouseCommand
                {
                    Id = guid,
                    NewAddress = "testStreet",
                    NewPostalCode = "49884",
                    NewCity = "Breda",
                    NewRegion = "Zeeland",
                    NewCountry = "Netherlands"
                };

                var handler = new WarehouseCommandHandler<MoveWarehouseCommand>(
                    genericRepository,
                    Mock.Of<ILogger<WarehouseCommandHandler<MoveWarehouseCommand>>>());

                // Act
                var result = await handler.HandleAsync(command);
                var value = result.Values.First();

                // Assert
                Assert.True(value);
                Assert.True(result.Keys.Count == 1);
                Assert.True(result.Values.Count == 1);
                Assert.Equal(guid, result.Keys.First());

                // Assert
                var moved = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(moved);
                Assert.Equal("testStreet", moved.Address.Street);
                Assert.Equal("49884", moved.Address.PostalCode);
                Assert.Equal("Netherlands", moved.Address.Country);
                Assert.Equal("Zeeland", moved.Address.Region);

                // If everything is fine, commit the transaction
                transaction.Commit();
            }
            finally
            {
                // Roll back the transaction to undo any changes made during the test
                transaction.Rollback();
            }
        }
    }
}
