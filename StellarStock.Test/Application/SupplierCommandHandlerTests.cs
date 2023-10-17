namespace StellarStock.Test.Application
{
    public class SupplierCommandHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public SupplierCommandHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            // clean up database
            _fixture.ClearData<InventoryItem>();
            _fixture.ClearData<Supplier>();
            _fixture.ClearData<Warehouse>();
        }

        [Fact]
        public async Task HandleAsync_CreateSupplierCommand_CallsHandleCreateAsync()
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
                var command = new CreateSupplierCommand
                {
                    Name = "Test Supplier",
                    Phone = "123456789",
                    ContactEmail = "test@example.com",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14))
                };

                var handlerMock = new Mock<ISupplierCommandHandler<CreateSupplierCommand>>();
                var repoMock = new Mock<IGenericRepository<Supplier>>();
                var handler = new SupplierCommandHandler<CreateSupplierCommand>(
                    repoMock.Object,
                    Mock.Of<ILogger<SupplierCommandHandler<CreateSupplierCommand>>>());

                // Act
                await handler.HandleAsync(command);

                // Assert
                repoMock.Verify(repo => repo.AddAsync(It.IsAny<Supplier>()), Times.Once);

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
        public async Task HandleAsync_UpdateSupplierCommand_CallsHandleUpdateAsync()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, logger);

                var guid = Guid.NewGuid().ToString();
                var supplier = new Supplier()
                {
                    Id = guid,
                    Name = "Test Supplier",
                    Phone = "123456789",
                    ContactEmail = "test@example.com",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14)),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await genericRepository.AddAsync(supplier);

                var x = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(x);
                Assert.True(x.Id == guid);

                var command = new UpdateSupplierCommand
                {
                    Id = guid,
                    NewName = "UpdatedSupplier",
                    NewPhone = "9876543210",
                    NewContactEmail = "example@test.com",
                    NewAddress = new AddressVO("newStreet", "newPostalCode", "testCity", "testRegion", "testCountry"),
                };

                var handler = new SupplierCommandHandler<UpdateSupplierCommand>(
                    genericRepository,
                    Mock.Of<ILogger<SupplierCommandHandler<UpdateSupplierCommand>>>());

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
                Assert.Equal("UpdatedSupplier", updated.Name);
                Assert.Equal("9876543210", updated.Phone);
                Assert.Equal("example@test.com", updated.ContactEmail);
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
        public async Task HandleAsync_DeleteSupplierCommand_CallsHandleDeleteAsync()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, logger);

                var handler = new SupplierCommandHandler<DeleteSupplierCommand>(
                genericRepository,
                Mock.Of<ILogger<SupplierCommandHandler<DeleteSupplierCommand>>>());

                var guid = Guid.NewGuid().ToString();
                var supplier = new Supplier()
                {
                    Id = guid,
                    Name = "Test Supplier",
                    Phone = "123456789",
                    ContactEmail = "test@example.com",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    ValidityPeriod = new DateRangeVO(new DateTime(2023, 8, 14), new DateTime(2023, 9, 14)),
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await genericRepository.AddAsync(supplier);

                var x = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(x);
                Assert.True(x.Id == guid);

                var command = new DeleteSupplierCommand
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
        public async Task HandleAsync_ActivateSupplierCommand_CallsHandleActivateAsync()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, logger);

                var guid = Guid.NewGuid().ToString();
                var supplier = new Supplier()
                {
                    Id = guid,
                    Name = "Test Supplier",
                    Phone = "123456789",
                    ContactEmail = "test@example.com",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 12, 14)),
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await genericRepository.AddAsync(supplier);

                var command = new ActivateSupplierCommand
                {
                    Id = guid,
                };

                var handler = new SupplierCommandHandler<ActivateSupplierCommand>(
                    genericRepository,
                    Mock.Of<ILogger<SupplierCommandHandler<ActivateSupplierCommand>>>());

                // Act
                var result = await handler.HandleAsync(command);

                // Assert
                Assert.True(result[guid]);
                Assert.True(result.Count == 1);
                Assert.True(result.Values.Count == 1);

                // Assert
                var activatedSupplier = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(activatedSupplier);
                Assert.True(activatedSupplier.IsActive);


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
        public async Task HandleAsync_DeactivateSupplierCommand_CallsHandleDeactivateAsync()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, logger);

                var guid = Guid.NewGuid().ToString();
                var supplier = new Supplier()
                {
                    Id = guid,
                    Name = "Test Supplier",
                    Phone = "123456789",
                    ContactEmail = "test@example.com",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 12, 14)),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await genericRepository.AddAsync(supplier);

                var command = new DeactivateSupplierCommand
                {
                    Id = guid,
                };

                var handler = new SupplierCommandHandler<DeactivateSupplierCommand>(
                    genericRepository,
                    Mock.Of<ILogger<SupplierCommandHandler<DeactivateSupplierCommand>>>());

                // Act
                var result = await handler.HandleAsync(command);

                // Assert
                Assert.True(result[guid]);
                Assert.True(result.Count == 1);
                Assert.True(result.Values.Count == 1);

                // Assert
                var deactivatedSupplier = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(deactivatedSupplier);
                Assert.False(deactivatedSupplier.IsActive);

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
        public async Task HandleAsync_UpdateSupplierCommand_LogsSuccessfulUpdate()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var loggerMock = new Mock<ILogger<SupplierCommandHandler<UpdateSupplierCommand>>>();
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, Mock.Of<ILogger<EFGenericRepository<Supplier>>>());

                var guid = Guid.NewGuid().ToString();

                var supplier = new Supplier()
                {
                    Id = guid,
                    Name = "Test Supplier",
                    Phone = "123456789",
                    ContactEmail = "test@example.com",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    ValidityPeriod = new DateRangeVO(DateTime.UtcNow, new DateTime(2023, 12, 14)),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await genericRepository.AddAsync(supplier);

                var x = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(x);
                Assert.True(x.Id == guid);

                var command = new UpdateSupplierCommand
                {
                    Id = guid,
                    NewName = "UpdatedSupplier",
                    NewPhone = "9876543210",
                    NewContactEmail = "example@test.com",
                    NewAddress = new AddressVO("newStreet", "newPostalCode", "testCity", "testRegion", "testCountry"),
                };

                var handler = new SupplierCommandHandler<UpdateSupplierCommand>(
            genericRepository,
            loggerMock.Object);

                // Act
                var result = await handler.HandleAsync(command);

                // Assert
                loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(), // Use It.IsAny<EventId>() here
                        It.Is<It.IsAnyType>((o, t) => string.Equals($"Supplier updated: {supplier.Id}", o.ToString(), StringComparison.OrdinalIgnoreCase)),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                    Times.Once);

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
        public async Task HandleAsync_DeleteSupplierCommand_LogsSuccessfulDeletion()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var loggerMock = new Mock<ILogger<SupplierCommandHandler<DeleteSupplierCommand>>>();
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, Mock.Of<ILogger<EFGenericRepository<Supplier>>>());

                var guid = Guid.NewGuid().ToString();

                var supplier = new Supplier()
                {
                    Id = guid,
                    Name = "Test Supplier",
                    Phone = "123456789",
                    ContactEmail = "test@example.com",
                    Address = new AddressVO("testStreet", "testCode", "testCity", "testRegion", "testCountry"),
                    ValidityPeriod = new DateRangeVO(new DateTime(2022, 11, 14), new DateTime(2022, 12, 14)),
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await genericRepository.AddAsync(supplier);

                var x = await genericRepository.GetByIdAsync(guid);
                Assert.NotNull(x);
                Assert.True(x.Id == guid);

                var command = new DeleteSupplierCommand
                {
                    Id = guid,
                };

                var handler = new SupplierCommandHandler<DeleteSupplierCommand>(
                    genericRepository,
                    loggerMock.Object);

                // Act
                var result = await handler.HandleAsync(command);

                // Assert
                loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((o, t) => string.Equals($"Supplier deleted: {supplier.Id}", o.ToString(), StringComparison.OrdinalIgnoreCase)),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                    Times.Once);

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
        public async Task HandleAsync_UpdateSupplierCommand_LogsFailedUpdate()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var loggerMock = new Mock<ILogger<SupplierCommandHandler<UpdateSupplierCommand>>>();
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, Mock.Of<ILogger<EFGenericRepository<Supplier>>>());

                var guid = Guid.NewGuid().ToString();

                // Do not add the supplier to the repository

                var command = new UpdateSupplierCommand
                {
                    Id = guid,
                    NewName = "UpdatedSupplier",
                    NewPhone = "9876543210",
                    NewContactEmail = "example@test.com",
                    NewAddress = new AddressVO("newStreet", "newPostalCode", "testCity", "testRegion", "testCountry"),
                };

                var handler = new SupplierCommandHandler<UpdateSupplierCommand>(
                    genericRepository,
                    loggerMock.Object);

                // Act
                var result = await handler.HandleAsync(command);

                // Assert
                loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((o, t) => string.Equals($"Supplier updated failed: {guid}", o.ToString(), StringComparison.OrdinalIgnoreCase)),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                    Times.Once);

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
        public async Task HandleAsync_DeleteSupplierCommand_LogsFailedDeletion()
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
                var unitOfWork = new TestUnitOfWork(context);
                var logger = new Mock<ILogger<EFGenericRepository<Supplier>>>().Object;
                var loggerMock = new Mock<ILogger<SupplierCommandHandler<DeleteSupplierCommand>>>();
                var genericRepository = new EFGenericRepository<Supplier>(unitOfWork, Mock.Of<ILogger<EFGenericRepository<Supplier>>>());

                var guid = Guid.NewGuid().ToString();

                // Do not add the supplier to the repository

                var command = new DeleteSupplierCommand
                {
                    Id = guid,
                };

                var handler = new SupplierCommandHandler<DeleteSupplierCommand>(
                    genericRepository,
                    loggerMock.Object);

                // Act
                var result = await handler.HandleAsync(command);

                // Assert
                loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((o, t) => string.Equals($"Supplier deleting failed: {guid}", o.ToString(), StringComparison.OrdinalIgnoreCase)),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                    Times.Once);

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