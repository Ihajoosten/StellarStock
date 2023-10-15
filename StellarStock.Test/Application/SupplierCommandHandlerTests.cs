namespace StellarStock.Test.Application
{
    public class SupplierCommandHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        public SupplierCommandHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task HandleCreateSupplierAsync_ShouldCreateSupplierSuccessfully()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<CreateSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<CreateSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var command = new CreateSupplierCommand
            {
                Name = "Test Supplier",
                Phone = "123456789",
                ContactEmail = "test@example.com",
                Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14))
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Supplier>()), Times.Once);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier created: {result}", capturedLogMessages);
        }

        [Fact]
        public async Task HandleUpdateSupplierAsync_ShouldUpdateSupplierSuccessfully()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<UpdateSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<UpdateSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var existingSupplier = new Supplier
            {
                Id = existingSupplierId,
                Name = "Test Name",
                Phone = "012345678910",
                ContactEmail = "example@test.nl",
                Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                IsActive = true,
                ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            mockRepository.Setup(repo => repo.AddAsync(existingSupplier));

            // Set up an existing supplier in the repository
            mockRepository.Setup(repo => repo.GetByIdAsync(existingSupplierId))
                .ReturnsAsync(existingSupplier);

            var command = new UpdateSupplierCommand
            {
                SupplierId = existingSupplierId,
                NewName = "Updated Supplier",
                NewPhone = "987654321",
                NewContactEmail = "updated@example.com",
                NewAddress = new AddressVO("newStreet", "newPostalCode", "newCity", "newRegion", "newCountry")
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Supplier>()), Times.Once);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier updated: {result}", capturedLogMessages);
        }

        [Fact]
        public async Task HandleDeleteSupplierAsync_ShouldDeleteSupplierSuccessfully()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<DeleteSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<DeleteSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var existingSupplier = new Supplier
            {
                Id = existingSupplierId,
                Name = "Test Name",
                Phone = "012345678910",
                ContactEmail = "example@test.nl",
                Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                IsActive = true,
                ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            mockRepository.Setup(repo => repo.AddAsync(existingSupplier));

            // Set up an existing supplier in the repository
            mockRepository.Setup(repo => repo.GetByIdAsync(existingSupplierId))
                .ReturnsAsync(existingSupplier);

            var command = new DeleteSupplierCommand
            {
                Id = existingSupplierId
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.RemoveAsync(existingSupplierId), Times.Once);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier removed: {result}", capturedLogMessages);
        }

        [Fact]
        public async Task HandleUpdateSupplierAsync_ShouldNotUpdateSupplier()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<UpdateSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<UpdateSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var command = new UpdateSupplierCommand
            {
                SupplierId = existingSupplierId,
                NewName = "Updated Supplier",
                NewPhone = "987654321",
                NewContactEmail = "updated@example.com",
                NewAddress = new AddressVO("newStreet", "newPostalCode", "newCity", "newRegion", "newCountry")
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Supplier>()), Times.Never);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier updated failed - could not find supplier: {result}", capturedLogMessages);
        }

        [Fact]
        public async Task HandleDeleteSupplierAsync_ShouldNotDeleteSupplier()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<DeleteSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<DeleteSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var command = new DeleteSupplierCommand
            {
                Id = existingSupplierId
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.RemoveAsync(existingSupplierId), Times.Never);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier removal failed: {result}", capturedLogMessages);
        }

        [Fact]
        public async Task HandleActivateSupplierAsync_ShouldActivateSupplierSuccessfully()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<ActivateSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<ActivateSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var existingSupplier = new Supplier
            {
                Id = existingSupplierId,
                Name = "Test Name",
                Phone = "012345678910",
                ContactEmail = "example@test.nl",
                Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                IsActive = false,
                ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            mockRepository.Setup(repo => repo.AddAsync(existingSupplier));

            // Set up an existing supplier in the repository
            mockRepository.Setup(repo => repo.GetByIdAsync(existingSupplierId))
                .ReturnsAsync(existingSupplier);

            var command = new ActivateSupplierCommand
            {
                SupplierId = existingSupplierId
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Supplier>()), Times.Once);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier activated: {existingSupplierId}", capturedLogMessages);

            // Verify that the result matches the supplier ID
            Assert.Equal(existingSupplierId, result);
        }

        [Fact]
        public async Task HandleActivateSupplierAsync_ShouldNotActivateSupplier()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<ActivateSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<ActivateSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var command = new ActivateSupplierCommand
            {
                SupplierId = existingSupplierId
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Supplier>()), Times.Never);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier activation failed: {existingSupplierId}", capturedLogMessages);

            // Verify that the result matches the supplier ID
            Assert.Equal(existingSupplierId, result);
        }

        [Fact]
        public async Task HandleActivateSupplierAsync_ShouldDeactivateSupplierSuccessfully()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<DeactivateSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<DeactivateSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var existingSupplier = new Supplier
            {
                Id = existingSupplierId,
                Name = "Test Name",
                Phone = "012345678910",
                ContactEmail = "example@test.nl",
                Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                IsActive = true,
                ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            mockRepository.Setup(repo => repo.AddAsync(existingSupplier));

            // Set up an existing supplier in the repository
            mockRepository.Setup(repo => repo.GetByIdAsync(existingSupplierId))
                .ReturnsAsync(existingSupplier);

            var command = new DeactivateSupplierCommand
            {
                SupplierId = existingSupplierId
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Supplier>()), Times.Once);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier deactivated: {existingSupplierId}", capturedLogMessages);

            // Verify that the result matches the supplier ID
            Assert.Equal(existingSupplierId, result);
        }

        [Fact]
        public async Task HandleActivateSupplierAsync_ShouldNotDeactivateSupplier()
        {
            _fixture.ClearData<Supplier>();

            // Arrange
            var existingSupplierId = "existing-supplier-id";
            var mockRepository = new Mock<IGenericRepository<Supplier>>();
            var mockLogger = new Mock<ILogger<SupplierCommandHandler<DeactivateSupplierCommand, Supplier>>>();
            var handler = new SupplierCommandHandler<DeactivateSupplierCommand, Supplier>(mockRepository.Object, mockLogger.Object);

            var command = new DeactivateSupplierCommand
            {
                SupplierId = existingSupplierId
            };

            // Capture log messages during the test
            var capturedLogMessages = new List<string>();
            mockLogger.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var message = invocation.Arguments[2]?.ToString();
                    capturedLogMessages.Add(message);
                }));

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(existingSupplierId), Times.Once);
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Supplier>()), Times.Never);

            // Verify that the log message contains the expected information
            Assert.Contains($"Supplier deactivation failed: {existingSupplierId}", capturedLogMessages);

            // Verify that the result matches the supplier ID
            Assert.Equal(existingSupplierId, result);
        }
    }
}
