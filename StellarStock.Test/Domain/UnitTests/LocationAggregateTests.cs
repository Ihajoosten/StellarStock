namespace StellarStock.Test.Domain.UnitTests
{
    public class LocationAggregateTests
    {
        [Fact]
        public void ThrowsArgumentNullException_With_InvalidLocation()
        {
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();

            // Assert & Act
            var exception = Assert.Throws<ArgumentNullException>(() => new LocationAggregate(null, inventoryItemRepositoryMock.Object));

            // Assert message
            Assert.Equal("Value cannot be null. (Parameter 'location')", exception.Message);
        }

        [Fact]
        public void CreateLocation_WithValidInputs_CreatesLocationAndRaisesEvents()
        {
            // Arrange
            var validLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(validLocation, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var locationOpenedEventHandler = new Mock<EventHandler<LocationOpenedEvent>>();
            var locationUpdatedEventHandler = new Mock<EventHandler<LocationUpdatedEvent>>();

            // Hook the mock event handlers to the events
            locationAggregate.LocationOpened += locationOpenedEventHandler.Object;
            locationAggregate.LocationUpdated += locationUpdatedEventHandler.Object;

            // Act
            locationAggregate.CreateLocation("New Store", "New Phone", new AddressVO("New street", "New postalcode", "New city", "New region", "New country"), true);

            // Assert

            // Verify that the location information is updated
            Assert.Equal("New Store", locationAggregate.Location.StoreName);
            Assert.Equal("New Phone", locationAggregate.Location.Phone);
            Assert.True(locationAggregate.Location.IsOpen);

            // Verify that the events were raised
            locationOpenedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<LocationOpenedEvent>()),
                Times.Once);

            locationUpdatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<LocationUpdatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void CreateLocation_WithInvalidInputs_ThrowsArgumentException()
        {
            // Arrange
            var invalidLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(invalidLocation, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                locationAggregate.CreateLocation("New Store", "New Phone", null, true));
        }

        [Fact]
        public void UpdateLocation_WithValidInputs_UpdatesLocationAndRaisesEvent()
        {
            // Arrange
            var validLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(validLocation, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var locationUpdatedEventHandler = new Mock<EventHandler<LocationUpdatedEvent>>();

            // Hook the mock event handler to the event
            locationAggregate.LocationUpdated += locationUpdatedEventHandler.Object;

            // Act
            locationAggregate.UpdateLocation("New Store", "New Phone", new AddressVO("street", "postalcode", "city", "region", "country"));

            // Assert

            // Verify that the location information is updated
            Assert.Equal("New Store", locationAggregate.Location.StoreName);
            Assert.Equal("New Phone", locationAggregate.Location.Phone);

            // Verify that the event was raised
            locationUpdatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<LocationUpdatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void UpdateLocation_WithNullNewName_ThrowsArgumentException()
        {
            // Arrange
            var validLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(validLocation, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                locationAggregate.UpdateLocation(null, "New Phone", new AddressVO("street", "postalcode", "city", "region", "country")));
        }

        [Fact]
        public void UpdateLocation_WithNullNewPhone_ThrowsArgumentException()
        {
            // Arrange
            var validLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(validLocation, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                locationAggregate.UpdateLocation("New Store", null, new AddressVO("street", "postalcode", "city", "region", "country")));
        }

        [Fact]
        public void MoveLocation_WithValidInputs_UpdatesLocationAndRaisesEvent()
        {
            // Arrange
            var validLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(validLocation, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var locationMovedEventHandler = new Mock<EventHandler<LocationMovedEvent>>();

            // Hook the mock event handler to the event
            locationAggregate.LocationMoved += locationMovedEventHandler.Object;

            // Act
            locationAggregate.MoveLocation("New Address", "New City", "New Region", "New Country", "New Postal Code");

            // Assert

            // Verify that the location information is updated
            Assert.Equal("New Address", locationAggregate.Location.LocationAddress.Street);
            Assert.Equal("New City", locationAggregate.Location.LocationAddress.City);
            Assert.Equal("New Region", locationAggregate.Location.LocationAddress.Region);
            Assert.Equal("New Country", locationAggregate.Location.LocationAddress.Country);
            Assert.Equal("New Postal Code", locationAggregate.Location.LocationAddress.PostalCode);

            // Verify that the event was raised
            locationMovedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<LocationMovedEvent>()),
                Times.Once);
        }

        [Fact]
        public void MoveLocation_WithNullNewAddress_ThrowsArgumentException()
        {
            // Arrange
            var validLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(validLocation, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                locationAggregate.MoveLocation(null, "New City", "New Region", "New Country", "New Postal Code"));
        }

        [Fact]
        public void MoveLocation_WithInvalidNewCity_ThrowsArgumentException()
        {
            // Arrange
            var validLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(validLocation, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                locationAggregate.MoveLocation("New Address", null, "New Region", "New Country", "New Postal Code"));
        }

        [Fact]
        public void CloseLocation_WithOpenLocation_ClosesLocationAndRaisesEvent()
        {
            // Arrange
            var openLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(openLocation, inventoryItemRepositoryMock.Object);

            // Mock the event handler
            var locationClosedEventHandler = new Mock<EventHandler<LocationClosedEvent>>();

            // Hook the mock event handler to the event
            locationAggregate.LocationClosed += locationClosedEventHandler.Object;

            // Act
            locationAggregate.CloseLocation(true);

            // Assert

            // Verify that the location information is updated
            Assert.False(locationAggregate.Location.IsOpen);

            // Verify that the event was raised
            locationClosedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<LocationClosedEvent>()),
                Times.Once);
        }

        [Fact]
        public void CloseLocation_WithClosedLocation_ThrowsArgumentException()
        {
            // Arrange
            var closedLocation = new Location
            {
                Id = new Guid().ToString(),
                StoreName = "name",
                Phone = "phone",
                LocationAddress = new AddressVO("street", "postalcode", "city", "region", "country"),
                IsOpen = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            var locationAggregate = new LocationAggregate(closedLocation, inventoryItemRepositoryMock.Object);

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                locationAggregate.CloseLocation(false));
        }
    }
}
