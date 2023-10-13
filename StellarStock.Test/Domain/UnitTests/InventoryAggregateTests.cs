using StellarStock.Domain.Aggregates;
using StellarStock.Domain.Entities;
using StellarStock.Domain.Events.ItemEvents;

namespace StellarStock.Test.Domain.UnitTests
{
    public class InventoryAggregateTests
    {
        // Fact Test
        [Fact]
        public void CanCreateInventoryItem()
        {
            // Arrange
            var inventory = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            // Act
            inventory.CreateInventoryItem("Test Item", "Description", ItemCategory.Tablet, 20, new ProductCodeVO("asfasdf-654wer-asdfa"), new QuantityVO(30),
                new MoneyVO(3, "EUR"), "12asdf-adsf234-asdf234", "a235sd-adsa23j-ap13pbw", new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)));

            // Assert
            Assert.NotNull(inventory.InventoryItem);
            Assert.Equal("Test Item", inventory.InventoryItem.Name);
            Assert.Equal("Description", inventory.InventoryItem.Description);
            Assert.Equal(ItemCategory.Tablet, inventory.InventoryItem.Category);
            Assert.Equal(20, inventory.InventoryItem.PopularityScore);
            Assert.Equal("asfasdf-654wer-asdfa", inventory.InventoryItem.ProductCode.Code);
            Assert.Equal(30, inventory.InventoryItem.Quantity.Value);
            Assert.Equal(3, inventory.InventoryItem.Money.Amount);
            Assert.Equal("EUR", inventory.InventoryItem.Money.Currency);
            Assert.Equal("12asdf-adsf234-asdf234", inventory.InventoryItem.LocationId);
        }

        // Theory Test
        [Theory]
        [InlineData(5, 15)] // Test updating popularity score from 5 to 15
        [InlineData(10, 20)] // Test updating popularity score from 10 to 20
        public void CanUpdatePopularityScore(int initialPopularity, int newPopularity)
        {
            // Arrange
            var inventory = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = initialPopularity,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            inventory.CreateInventoryItem("Test Item", "Description", ItemCategory.Tablet, initialPopularity, new ProductCodeVO("asfasdf-654wer-asdfa"), new QuantityVO(30),
                new MoneyVO(3, "EUR"), "12asdf-adsf234-asdf234", "a235sd-adsa23j-ap13pbw", new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)));

            // Act
            inventory.UpdatePopularityScore(newPopularity);

            // Assert
            Assert.Equal(newPopularity, inventory.InventoryItem.PopularityScore);
        }

        // Theory Test with Exception
        [Theory]
        [InlineData("12asdf-adsf234-asdf234", "8yip87-2hiuvx-cvvwws")]
        [InlineData("12asdf-adsf234-asdf234", "sdf4we-pbvp2s-7svase")]
        public void CanNotMoveExpiredItem(string initialLocation, string newLocation)
        {
            // Arrange
            var inventory = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(new DateTime(2023, 9, 21), new DateTime(2023, 9, 21)),
                LocationId = initialLocation,
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            inventory.CreateInventoryItem("Test Item", "Description", ItemCategory.Tablet, 20, new ProductCodeVO("asfasdf-654wer-asdfa"), new QuantityVO(30),
                new MoneyVO(3, "EUR"), initialLocation, "a235sd-adsa23j-ap13pbw", new DateRangeVO(new DateTime(2023, 9, 21), new DateTime(2023, 9, 21)));

            // Assert & Act
            var exception = Assert.Throws<InvalidOperationException>(() => inventory.MoveItem(newLocation));

            // Assert message
            Assert.Equal("Cannot move an expired item.", exception.Message);
        }

        // Theory Test with Exception
        [Theory]
        [InlineData("12asdf-adsf234-asdf234", null)]
        [InlineData("12asdf-adsf234-asdf234", "")]
        public void InvalidLocationThrowsException(string initialLocation, string newLocation)
        {
            // Arrange
            var inventory = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = initialLocation,
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            inventory.CreateInventoryItem("Test Item", "Description", ItemCategory.Tablet, 20, new ProductCodeVO("asfasdf-654wer-asdfa"), new QuantityVO(30),
                new MoneyVO(3, "EUR"), initialLocation, "a235sd-adsa23j-ap13pbw", new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)));

            // Assert & Act
            var exception = Assert.Throws<ArgumentException>(() => inventory.MoveItem(newLocation));

            // Assert message
            Assert.Equal("New location ID cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void UpdateQuantity_WithNegativeQuantity_ThrowsArgumentException()
        {
            // Arrange
            var invalidQuantity = -5;
            var inventoryAggregate = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => inventoryAggregate.UpdateQuantity(invalidQuantity));
            Assert.Equal("Quantity must be a non-negative value.", exception.Message);
        }

        [Fact]
        public void UpdateQuantity_WithExcessiveQuantityChange_ThrowsInvalidOperationException()
        {
            // Arrange
            var initialQuantity = 10;
            var newQuantity = initialQuantity + 200; // Change exceeds allowed limit
            var inventoryAggregate = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => inventoryAggregate.UpdateQuantity(newQuantity));
            Assert.Equal("Quantity change exceeds allowed limit.", exception.Message);
        }

        [Fact]
        public void ExpireItem_WithAlreadyExpiredItem_ThrowsInvalidOperationException()
        {
            // Arrange
            var expiredValidityPeriod = new DateRangeVO(new DateTime(2023, 9, 21), new DateTime(2023, 9, 21));
            var inventoryAggregate = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = expiredValidityPeriod,
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => inventoryAggregate.ExpireItem());
            Assert.Equal("The item is already expired.", exception.Message);
        }

        [Fact]
        public void UpdateItem_WithValidInput_UpdatesItemAndRaisesEvent()
        {
            // Arrange
            var inventoryAggregate = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            var newName = "New Item Name";
            var newDescription = "Updated item description.";
            var newPopularityScore = 20; // Assuming 20 is a valid popularity score
            var newQuantity = new QuantityVO(50); // Assuming 50 is a valid quantity
            var newMoney = new MoneyVO(10.99m, "USD"); // Assuming 10.99 USD is a valid money value

            // Act
            inventoryAggregate.UpdateItem(newName, newDescription, newPopularityScore, newQuantity, newMoney);

            // Assert
            Assert.Equal(newName, inventoryAggregate.InventoryItem.Name);
            Assert.Equal(newDescription, inventoryAggregate.InventoryItem.Description);
            Assert.Equal(newPopularityScore, inventoryAggregate.InventoryItem.PopularityScore);
            Assert.Equal(newQuantity, inventoryAggregate.InventoryItem.Quantity);
            Assert.Equal(newMoney, inventoryAggregate.InventoryItem.Money);
        }

        [Fact]
        public void UpdateItem_WithInvalidNameInput_ThrowsException()
        {
            // Arrange
            var inventoryAggregate = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            var newName = string.Empty; // Invalid input
            var newDescription = "Updated item description.";
            var newPopularityScore = 15; 
            var newQuantity = new QuantityVO(10);
            var newMoney = new MoneyVO(10.99m, "USD");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                inventoryAggregate.UpdateItem(newName, newDescription, newPopularityScore, newQuantity, newMoney));

            // Assert
            Assert.Equal("New name cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void UpdateItem_WithInvalidPopularityScoreInput_ThrowsException()
        {
            // Arrange
            var inventoryAggregate = new InventoryAggregate(new InventoryItem()
            {
                Id = new Guid().ToString(),
                Name = "Test Item",
                Description = "Description",
                Category = ItemCategory.Tablet,
                PopularityScore = 20,
                ProductCode = new ProductCodeVO("asfasdf-654wer-asdfa"),
                Quantity = new QuantityVO(30),
                Money = new MoneyVO(3, "EUR"),
                ValidityPeriod = new DateRangeVO(DateTime.Now, new DateTime(2023, 10, 21)),
                LocationId = "12asdf-adsf234-asdf234",
                SupplierId = "a235sd-adsa23j-ap13pbw",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            var newName = "test test now"; // Invalid input
            var newDescription = "Updated item description.";
            var newQuantity = new QuantityVO(10);
            var newMoney = new MoneyVO(10.99m, "USD");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                inventoryAggregate.UpdateItem(newName, newDescription, -15, newQuantity, newMoney));

            // Assert
            Assert.Equal("New popularity score cannot be negative.", exception.Message);
        }
    }
}