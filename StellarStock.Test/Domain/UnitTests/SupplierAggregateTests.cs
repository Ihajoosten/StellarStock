namespace StellarStock.Test.Domain.UnitTests
{
    public class SupplierAggregateTests
    {
        [Fact]
        public void ThrowsArgumentNullException_With_InvalidSupplier()
        {
            // Assert & Act
            var exception = Assert.Throws<ArgumentNullException>(() => new SupplierAggregate(null));

            // Assert message
            Assert.Equal("Value cannot be null. (Parameter 'supplier')", exception.Message);
        }

        [Fact]
        public void CreateSupplier_WithValidInput_CreatesSupplierAndRaisesEvents()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Mock the event handler
            var supplierCreatedEventHandler = new Mock<EventHandler<SupplierCreatedEvent>>();
            var supplierUpdatedEventHandler = new Mock<EventHandler<SupplierUpdatedEvent>>();

            // Hook the mock event handler to the event
            supplierAggregate.SupplierCreated += supplierCreatedEventHandler.Object;
            supplierAggregate.SupplierUpdated += supplierUpdatedEventHandler.Object;

            // Act
            supplierAggregate.CreateSupplier(name, phone, email, address, isActive, validityPeriod);

            // Assert
            Assert.NotNull(supplierAggregate.Supplier);
            Assert.Equal(name, supplierAggregate.Supplier.Name);
            Assert.Equal(phone, supplierAggregate.Supplier.Phone);
            Assert.Equal(email, supplierAggregate.Supplier.ContactEmail);
            Assert.Equal(address, supplierAggregate.Supplier.Address);
            Assert.True(supplierAggregate.Supplier.IsActive);
            Assert.Equal(validityPeriod, supplierAggregate.Supplier.ValidityPeriod);

            // Verify that the events were raised
            supplierCreatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<SupplierCreatedEvent>()),
                Times.Once);

            supplierUpdatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<SupplierUpdatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void UpdateSupplier_WithValidInput_UpdatesSupplierAndRaisesEvent()
        {
            // Arrange
            var newStoreName = "New Store Name";
            var newPhone = "987654321";
            var newEmail = "newemail@example.com";
            var newAddress = new AddressVO("New Street", "New City", "New Region", "New Country", "New PostalCode");

            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));
            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = "name",
                Phone = "phone",
                ContactEmail = "email@test.nl",
                Address = new AddressVO("Old Street", "Old City", "Old Region", "Old Country", "Old PostalCode"),
                IsActive = true,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Mock the event handler
            var supplierUpdatedEventHandler = new Mock<EventHandler<SupplierUpdatedEvent>>();

            // Hook the mock event handler to the event
            supplierAggregate.SupplierUpdated += supplierUpdatedEventHandler.Object;

            // Act
            supplierAggregate.UpdateSupplier(newStoreName, newPhone, newEmail, newAddress);

            // Assert
            Assert.Equal(newStoreName, supplierAggregate.Supplier.Name);
            Assert.Equal(newPhone, supplierAggregate.Supplier.Phone);
            Assert.Equal(newEmail, supplierAggregate.Supplier.ContactEmail);
            Assert.Equal(newAddress, supplierAggregate.Supplier.Address);

            // Verify that the event was raised
            supplierUpdatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<SupplierUpdatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void DeleteSupplier_WithValidSupplier_RaisesEvent()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Mock the event handler
            var supplierDeletedEventHandler = new Mock<EventHandler<SupplierDeletedEvent>>();

            // Hook the mock event handler to the event
            supplierAggregate.SupplierDeleted += supplierDeletedEventHandler.Object;

            // Act
            supplierAggregate.DeleteSupplier();

            // Assert

            // Verify that the event was raised
            supplierDeletedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<SupplierDeletedEvent>()),
                Times.Once);
        }

        [Fact]
        public void ActivateSupplier_WithInactiveSupplier_ActivatesSupplierAndRaisesEvent()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = false;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Mock the event handler
            var supplierActivatedEventHandler = new Mock<EventHandler<SupplierActivatedEvent>>();

            // Hook the mock event handler to the event
            supplierAggregate.SupplierActivated += supplierActivatedEventHandler.Object;

            // Act
            supplierAggregate.ActivateSupplier();

            // Assert

            // Verify that the supplier is activated
            Assert.True(supplierAggregate.Supplier.IsActive);

            // Verify that the event was raised
            supplierActivatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<SupplierActivatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void DeactivateSupplier_WithActiveSupplier_DeactivatesSupplierAndRaisesEvent()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Mock the event handler
            var supplierDeactivatedEventHandler = new Mock<EventHandler<SupplierDeactivatedEvent>>();

            // Hook the mock event handler to the event
            supplierAggregate.SupplierDeactivated += supplierDeactivatedEventHandler.Object;

            // Act
            supplierAggregate.DeactivateSupplier();

            // Assert

            // Verify that the supplier is deactivated
            Assert.False(supplierAggregate.Supplier.IsActive);

            // Verify that the event was raised
            supplierDeactivatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<SupplierDeactivatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void UpdateSupplier_WithValidInputs_UpdatesSupplierAndRaisesEvent()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Mock the event handler
            var supplierUpdatedEventHandler = new Mock<EventHandler<SupplierUpdatedEvent>>();

            // Hook the mock event handler to the event
            supplierAggregate.SupplierUpdated += supplierUpdatedEventHandler.Object;

            // Act
            supplierAggregate.UpdateSupplier("New Store", "New Phone", "new@email.com", address);

            // Assert

            // Verify that the supplier information is updated
            Assert.Equal("New Store", supplierAggregate.Supplier.Name);
            Assert.Equal("New Phone", supplierAggregate.Supplier.Phone);
            Assert.Equal("new@email.com", supplierAggregate.Supplier.ContactEmail);

            // Verify that the event was raised
            supplierUpdatedEventHandler.Verify(
                handler => handler(It.IsAny<object>(), It.IsAny<SupplierUpdatedEvent>()),
                Times.Once);
        }

        [Fact]
        public void UpdateSupplier_WithNullNewName_ThrowsArgumentException()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                supplierAggregate.UpdateSupplier(null, "New Phone", "new@email.com", address));
        }

        [Fact]
        public void UpdateSupplier_WithNullNewPhone_ThrowsArgumentException()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                supplierAggregate.UpdateSupplier("New Store", null, "new@email.com", address));
        }

        [Fact]
        public void UpdateSupplier_WithNullNewEmail_ThrowsArgumentException()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
                supplierAggregate.UpdateSupplier("New Store", "New Phone", null, address));
        }

        [Fact]
        public void UpdateSupplier_WithInvalidNewAddress_ThrowsArgumentException()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Act and Assert
            Assert.Throws<NullReferenceException>(() =>
                supplierAggregate.UpdateSupplier("New Store", "New Phone", "new@email.com", null));
        }

        [Fact]
        public void UpdateSupplier_WithExpiredSupplier_ThrowsInvalidOperationException()
        {
            // Arrange
            var name = "SupplierName";
            var phone = "123456789";
            var email = "supplier@example.com";
            var address = new AddressVO("Street", "City", "Region", "Country", "PostalCode");
            var isActive = true;
            var validityPeriod = new DateRangeVO(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1).AddDays(30));

            var supplierAggregate = new SupplierAggregate(new Supplier
            {
                Id = new Guid().ToString(),
                Name = name,
                Phone = phone,
                ContactEmail = email,
                Address = address,
                IsActive = isActive,
                ValidityPeriod = validityPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() =>
                supplierAggregate.UpdateSupplier("New Store", "New Phone", "new@email.com", address));
        }
    }
}
