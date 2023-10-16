using Microsoft.Extensions.Logging.Abstractions;

namespace StellarStock.Test.Application
{
    public class InventoryCommandHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public InventoryCommandHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            // clean up database
            _fixture.ClearData<InventoryItem>();
            _fixture.ClearData<Supplier>();
            _fixture.ClearData<Warehouse>();
        }

        [Fact]
        public void VerifyTestDbContext()
        {
            Assert.NotNull(_fixture.Context);
            Assert.IsType<TestDbContext>(_fixture.Context);
        }

        [Fact]
        public async Task HandleCreateAsync_ValidCommand_CallsRepository()
        {
            // Arrange
            var repositoryMock = new Mock<IGenericRepository<InventoryItem>>();
            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<InventoryItem>())).ReturnsAsync(true);

            var logger = new Logger<InventoryItemCommandHandler<CreateInventoryItemCommand>>(new NullLoggerFactory());
            var handler = new InventoryItemCommandHandler<CreateInventoryItemCommand>(repositoryMock.Object, logger);

            var createCommand = new CreateInventoryItemCommand
            {

                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2025, 10, 21))
            };

            // Act
            var result = await handler.HandleCreateAsync(createCommand);
            var key = result.Keys.First();
            var value = result.Values.First();

            // Assert
            Assert.True(value);
            Assert.True(result.Keys.Count == 1);
            Assert.True(result.Values.Count == 1);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<InventoryItem>()), Times.Once);
        }

        [Fact]
        public async Task HandleCreateAsync_ValidCommand_LogsInformation()
        {
            // Arrange
            var repositoryMock = new Mock<IGenericRepository<InventoryItem>>();
            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<InventoryItem>())).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<InventoryItemCommandHandler<CreateInventoryItemCommand>>>();
            loggerMock.Setup(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
            ));

            var handler = new InventoryItemCommandHandler<CreateInventoryItemCommand>(repositoryMock.Object, loggerMock.Object);

            var createCommand = new CreateInventoryItemCommand
            {

                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2025, 10, 21))
            };

            // Act
            var result = await handler.HandleCreateAsync(createCommand);
            var key = result.Keys.First();
            var value = result.Values.First();

            // Assert
            Assert.True(value);
            Assert.True(result.Keys.Count == 1);
            Assert.True(result.Values.Count == 1);

            // Assert
            loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Inventory item created: {key}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
            ), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_CreateInventoryItemCommand_CallsHandleCreateAsync()
        {
            // Arrange
            var command = new CreateInventoryItemCommand
            {

                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2025, 10, 21))
            };

            var repoMock = new Mock<IGenericRepository<InventoryItem>>();
            var loggerMock = new Mock<ILogger<InventoryItemCommandHandler<CreateInventoryItemCommand>>>();

            CreateInventoryItemCommand capturedCommand = null;

            repoMock.Setup(repo => repo.AddAsync(It.IsAny<InventoryItem>()))
                .Callback<InventoryItem>(item => capturedCommand = command)
                .ReturnsAsync(true);

            var commandHandler = new InventoryItemCommandHandler<CreateInventoryItemCommand>(
                repoMock.Object,
                loggerMock.Object);

            // Act
            await commandHandler.HandleAsync(command);

            // Assert
            Assert.NotNull(capturedCommand);
            Assert.Equal("Test Item", capturedCommand.Name);
        }

        [Fact]
        public async Task HandleAsync_UpdateInventoryItemCommand_CallsHandleUpdateAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var genericRepository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            var guid = Guid.NewGuid().ToString();
            var newItem = new InventoryItem()
            {
                Id = guid,
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2025, 10, 21))
            };
            await genericRepository.AddAsync(newItem);

            var command = new UpdateInventoryItemCommand()
            {
                Id = guid,
                NewName = "UpdatedItem",
                NewDescription = "Description",
                NewCategory = ItemCategory.Wearable,
                NewPopularityScore = 50,
                NewProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                NewQuantity = new QuantityVO(20),
                NewMoney = new MoneyVO(3, "GBP"),
            };

            var commandHandler = new InventoryItemCommandHandler<UpdateInventoryItemCommand>(
                genericRepository,
                Mock.Of<ILogger<InventoryItemCommandHandler<UpdateInventoryItemCommand>>>());

            // Act
            var result = await commandHandler.HandleAsync(command);
            var value = result.Values.First();

            // Assert
            Assert.True(value);
            Assert.True(result.Keys.Count == 1);
            Assert.True(result.Values.Count == 1);
            Assert.Equal(guid, result.Keys.First());

            // Assert
            // Verify that UpdateAsync is called on the repository with the expected ID
            var updatedItem = await genericRepository.GetByIdAsync(guid);
            Assert.NotNull(updatedItem);
            Assert.Equal("UpdatedItem", updatedItem.Name);
            Assert.Equal(ItemCategory.Wearable, updatedItem.Category);
            Assert.Equal(50, updatedItem.PopularityScore);
            Assert.Equal("GBP", updatedItem.Money.Currency);
        }

        [Fact]
        public async Task HandleAsync_DeleteInventoryItemCommand_CallsHandleDeleteAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var genericRepository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            var guid = Guid.NewGuid().ToString();
            var newItem = new InventoryItem()
            {
                Id = guid,
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                ValidityPeriod = new DateRangeVO(new DateTime(2021, 10, 21), new DateTime(2022, 10, 21))
            };

            await genericRepository.AddAsync(newItem);

            var commandHandler = new InventoryItemCommandHandler<DeleteInventoryItemCommand>(
                genericRepository,
                Mock.Of<ILogger<InventoryItemCommandHandler<DeleteInventoryItemCommand>>>());

            var itemToDelete = await genericRepository.GetByIdAsync(guid);
            Assert.NotNull(itemToDelete);
            Assert.True(int.IsPositive(itemToDelete.Quantity.Value));

            var command = new DeleteInventoryItemCommand() { Id = guid };

            // Act
            var result = await commandHandler.HandleAsync(command);
            var value = result.Values.First();

            // Assert
            Assert.True(value);
            Assert.True(result.Keys.Count == 1);
            Assert.True(result.Values.Count == 1);
            Assert.Equal(guid, result.Keys.First());

            // Assert
            // Verify that RemoveAsync is called on the repository with the expected ID
            var deletedItem = await genericRepository.GetByIdAsync(guid);
            Assert.Null(deletedItem); // Ensure the item is deleted from the database
        }

        [Fact]
        public async Task HandleAsync_IncreaseQuantityCommand_CallsHandleIncreaseQuantityAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var genericRepository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            var guid = Guid.NewGuid().ToString();
            var newItem = new InventoryItem()
            {
                Id = guid,
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2025, 10, 21))
            };
            await genericRepository.AddAsync(newItem);

            // Arrange
            var command = new IncreaseInventoryItemQuantityCommand
            {
                Id = guid,
                Quantity = 10
            };

            var handlerMock = new Mock<IInventoryItemCommandHandler<IncreaseInventoryItemQuantityCommand>>();
            var repoMock = new Mock<IGenericRepository<InventoryItem>>();
            var commandHandler = new InventoryItemCommandHandler<IncreaseInventoryItemQuantityCommand>(
                repoMock.Object,
                Mock.Of<ILogger<InventoryItemCommandHandler<IncreaseInventoryItemQuantityCommand>>>());

            // Act
            await commandHandler.HandleAsync(command);

            // Assert
            repoMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
            repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<InventoryItem>())).ReturnsAsync(true);
        }

        [Fact]
        public async Task HandleAsync_DecreaseQuantityCommand_CallsHandleDecreaseQuantityAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new TestDbContext(options);
            var unitOfWork = new TestUnitOfWork(context);
            var logger = new Mock<ILogger<EFGenericRepository<InventoryItem>>>().Object;
            var genericRepository = new EFGenericRepository<InventoryItem>(unitOfWork, logger);

            var guid = Guid.NewGuid().ToString();
            var newItem = new InventoryItem()
            {
                Id = guid,
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                WarehouseId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2025, 10, 21))
            };
            await genericRepository.AddAsync(newItem);

            // Arrange
            var command = new DecreaseInventoryItemQuantityCommand
            {
                Id = guid,
                Quantity = 5
            };

            var handlerMock = new Mock<IInventoryItemCommandHandler<DecreaseInventoryItemQuantityCommand>>();
            var repoMock = new Mock<IGenericRepository<InventoryItem>>();
            var commandHandler = new InventoryItemCommandHandler<DecreaseInventoryItemQuantityCommand>(
                repoMock.Object,
                Mock.Of<ILogger<InventoryItemCommandHandler<DecreaseInventoryItemQuantityCommand>>>());

            // Act
            await commandHandler.HandleAsync(command);

            // Assert
            repoMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
            repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<InventoryItem>())).ReturnsAsync(true);
        }
    }
}