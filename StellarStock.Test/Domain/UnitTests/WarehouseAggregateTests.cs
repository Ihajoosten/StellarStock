namespace StellarStock.Test.Domain.UnitTests
{
    public class WarehouseAggregateTests
    {
        [Fact]
        public void ThrowsArgumentNullException_With_InvalidWarehouse()
        {
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();

            // Assert & Act
            var exception = Assert.Throws<ArgumentNullException>(() => new WarehouseAggregate(null, inventoryItemRepositoryMock.Object));

            // Assert message
            Assert.Equal("Value cannot be null. (Parameter 'warehouse')", exception.Message);
        }

        [Fact]
        public void CreateWarehouse_WithValidInputs_CreatesWarehouseAndRaisesEvents()
        {
            // Arrange
            var validWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(validWarehouse, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var WarehouseOpenedEventHandler = new Mock<EventHandler<WarehouseOpenedEvent>>();
            var WarehouseUpdatedEventHandler = new Mock<EventHandler<WarehouseUpdatedEvent>>();

            // Hook the mock event handlers to the events
            WarehouseAggregate.WarehouseOpened += WarehouseOpenedEventHandler.Object;
            WarehouseAggregate.WarehouseUpdated += WarehouseUpdatedEventHandler.Object;

            // Act
            WarehouseAggregate.CreateWarehouse("New Store", "New Phone", new AddressVO("New street", "New postalcode", "New city", "New region", "New country"), true);

            // Assert

            // Verify that the Warehouse information is updated
            Assert.Equal("New Store", WarehouseAggregate.Warehouse.Name);
            Assert.Equal("New Phone", WarehouseAggregate.Warehouse.Phone);
            Assert.True(WarehouseAggregate.Warehouse.IsOpen);

            // Verify that the events were raised
            WarehouseOpenedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<WarehouseOpenedEvent>()),
                Times.Once);

            WarehouseUpdatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<WarehouseUpdatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void CreateWarehouse_WithInvalidInputs_ThrowsArgumentException()
        {
            // Arrange
            var invalidWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(invalidWarehouse, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                WarehouseAggregate.CreateWarehouse("New Store", "New Phone", null, true));
        }

        [Fact]
        public void UpdateWarehouse_WithValidInputs_UpdatesWarehouseAndRaisesEvent()
        {
            // Arrange
            var validWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(validWarehouse, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var WarehouseUpdatedEventHandler = new Mock<EventHandler<WarehouseUpdatedEvent>>();

            // Hook the mock event handler to the event
            WarehouseAggregate.WarehouseUpdated += WarehouseUpdatedEventHandler.Object;

            // Act
            WarehouseAggregate.UpdateWarehouse("New Store", "New Phone", new AddressVO("street", "postalcode", "city", "region", "country"));

            // Assert

            // Verify that the Warehouse information is updated
            Assert.Equal("New Store", WarehouseAggregate.Warehouse.Name);
            Assert.Equal("New Phone", WarehouseAggregate.Warehouse.Phone);

            // Verify that the event was raised
            WarehouseUpdatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<WarehouseUpdatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void UpdateWarehouse_WithNullNewName_ThrowsArgumentException()
        {
            // Arrange
            var validWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(validWarehouse, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                WarehouseAggregate.UpdateWarehouse(null, "New Phone", new AddressVO("street", "postalcode", "city", "region", "country")));
        }

        [Fact]
        public void UpdateWarehouse_WithNullNewPhone_ThrowsArgumentException()
        {
            // Arrange
            var validWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(validWarehouse, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                WarehouseAggregate.UpdateWarehouse("New Store", null, new AddressVO("street", "postalcode", "city", "region", "country")));
        }

        [Fact]
        public void MoveWarehouse_WithValidInputs_UpdatesWarehouseAndRaisesEvent()
        {
            // Arrange
            var validWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(validWarehouse, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var WarehouseMovedEventHandler = new Mock<EventHandler<WarehouseMovedEvent>>();

            // Hook the mock event handler to the event
            WarehouseAggregate.WarehouseMoved += WarehouseMovedEventHandler.Object;

            // Act
            WarehouseAggregate.MoveWarehouse("New Address", "New City", "New Region", "New Country", "New Postal Code");

            // Assert

            // Verify that the Warehouse information is updated
            Assert.Equal("New Address", WarehouseAggregate.Warehouse.Address.Street);
            Assert.Equal("New City", WarehouseAggregate.Warehouse.Address.City);
            Assert.Equal("New Region", WarehouseAggregate.Warehouse.Address.Region);
            Assert.Equal("New Country", WarehouseAggregate.Warehouse.Address.Country);
            Assert.Equal("New Postal Code", WarehouseAggregate.Warehouse.Address.PostalCode);

            // Verify that the event was raised
            WarehouseMovedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<WarehouseMovedEvent>()),
                Times.Once);
        }

        [Fact]
        public void MoveWarehouse_WithNullNewAddress_ThrowsArgumentException()
        {
            // Arrange
            var validWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(validWarehouse, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                WarehouseAggregate.MoveWarehouse(null, "New City", "New Region", "New Country", "New Postal Code"));
        }

        [Fact]
        public void MoveWarehouse_WithInvalidNewCity_ThrowsArgumentException()
        {
            // Arrange
            var validWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(validWarehouse, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                WarehouseAggregate.MoveWarehouse("New Address", null, "New Region", "New Country", "New Postal Code"));
        }

        [Fact]
        public void CloseWarehouse_WithOpenWarehouse_ClosesWarehouseAndRaisesEvent()
        {
            // Arrange
            var openWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(openWarehouse, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var WarehouseClosedEventHandler = new Mock<EventHandler<WarehouseClosedEvent>>();

            // Hook the mock event handler to the event
            WarehouseAggregate.WarehouseClosed += WarehouseClosedEventHandler.Object;

            // Act
            WarehouseAggregate.CloseWarehouse(true);

            // Assert

            // Verify that the Warehouse information is updated
            Assert.False(WarehouseAggregate.Warehouse.IsOpen);

            // Verify that the event was raised
            WarehouseClosedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<WarehouseClosedEvent>()),
                Times.Once);
        }

        [Fact]
        public void CloseWarehouse_WithClosedWarehouse_ThrowsArgumentException()
        {
            // Arrange
            var closedWarehouse = new Warehouse
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                Address = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var WarehouseAggregate = new WarehouseAggregate(closedWarehouse, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                WarehouseAggregate.CloseWarehouse(false));
        }
    }
}
