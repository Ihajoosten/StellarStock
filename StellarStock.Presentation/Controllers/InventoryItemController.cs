namespace StellarStock.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inventory-items")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class InventoryItemController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ILogger<InventoryItemController> _logger;

        public InventoryItemController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger<InventoryItemController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        /* :: InventoryItem Commands :: */
        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem([FromBody] CreateInventoryItemCommand command)
        {
            try
            {
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Inventory item created successfully");
                return result ? Ok("Inventory item created successfully") : UnprocessableEntity("Could not create inventory item");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateInventoryItem: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(string id, [FromBody] UpdateInventoryItemCommand command)
        {
            try
            {
                // Ensure the ID in the command matches the route parameter
                if (id != command.Id)
                {
                    _logger.LogError("Invalid ID in the request");
                    return BadRequest("Invalid ID in the request");
                }

                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Inventory item updated successfully");
                return result ? Ok("Inventory item updated successfully") : UnprocessableEntity("Could not update inventory item");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateInventoryItem: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(string id)
        {
            try
            {
                // Ensure the ID in the route parameter is not null or empty
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogError("Invalid ID in the request");
                    return BadRequest("Invalid ID in the request");
                }

                var command = new DeleteInventoryItemCommand { Id = id };
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Inventory item deleted successfully");
                return result ? Ok("Inventory item deleted successfully") : UnprocessableEntity("Could not delete inventory item");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteInventoryItem: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/increase-quantity")]
        public async Task<IActionResult> IncreaseInventoryItemQuantity(string id, [FromBody] IncreaseInventoryItemQuantityCommand command)
        {
            try
            {
                // Ensure the ID in the command matches the route parameter
                if (id != command.Id)
                {
                    _logger.LogError("Invalid ID in the request");
                    return BadRequest("Invalid ID in the request");
                }

                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Inventory item quantity increased successfully");
                return result ? Ok("Inventory item quantity increased successfully") : UnprocessableEntity("Could not increase inventory item quantity");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in IncreaseInventoryItemQuantity: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/decrease-quantity")]
        public async Task<IActionResult> DecreaseInventoryItemQuantity(string id, [FromBody] DecreaseInventoryItemQuantityCommand command)
        {
            try
            {
                // Ensure the ID in the command matches the route parameter
                if (id != command.Id)
                {
                    _logger.LogError("Invalid ID in the request");
                    return BadRequest("Invalid ID in the request");
                }

                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Inventory item quantity decreased successfully");
                return result ? Ok("Inventory item quantity decreased successfully") : UnprocessableEntity("Could not decrease inventory item quantity");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DecreaseInventoryItemQuantity: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /* :: InventoryItem Queries :: */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryItemById(string id)
        {
            try
            {
                var query = new GetInventoryItemByIdQuery<InventoryItemDto>() { Id = id };
                var result = await _queryDispatcher.DispatchAsync<GetInventoryItemByIdQuery<InventoryItemDto>, InventoryItemDto>(query);

                if (result != null)
                {
                    _logger.LogInformation($"Retrieved inventory item with ID {id} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"Inventory item with ID {id} not found");
                return NotFound($"Inventory item with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetInventoryItemById: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetInventoryItemsByCategory(string category)
        {
            try
            {
                if (Enum.TryParse<ItemCategory>(category, true, out var itemCategory))
                {
                    var query = new GetInventoryItemsByCategoryQuery<IEnumerable<InventoryItemDto>> { Category = itemCategory };
                    var result = await _queryDispatcher.DispatchAsync<GetInventoryItemsByCategoryQuery<IEnumerable<InventoryItemDto>>, IEnumerable<InventoryItemDto>>(query);

                    if (result != null && result.Any())
                    {
                        _logger.LogInformation($"Retrieved inventory items in category {category} successfully");
                        return Ok(result);
                    }

                    _logger.LogInformation($"No inventory items in category {category} found");
                    return NotFound($"No inventory items in category {category} found");
                }
                else
                {
                    _logger.LogWarning($"Invalid category value: {category}");
                    return BadRequest($"Invalid category value: {category}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetInventoryItemsByCategory: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<IActionResult> GetInventoryItemsByWarehouse(string warehouseId)
        {
            try
            {
                var query = new GetInventoryItemsByWarehouseQuery<IEnumerable<InventoryItemDto>> { Id = warehouseId };
                var result = await _queryDispatcher.DispatchAsync<GetInventoryItemsByWarehouseQuery<IEnumerable<InventoryItemDto>>, IEnumerable<InventoryItemDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation($"Retrieved inventory items in warehouse {warehouseId} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"No inventory items in warehouse {warehouseId} found");
                return NotFound($"No inventory items in warehouse {warehouseId} found");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetInventoryItemsByWarehouse: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("in-stock")]
        public async Task<IActionResult> GetInStockItems()
        {
            try
            {
                var query = new GetInStockItemsQuery<IEnumerable<InventoryItemDto>>();
                var result = await _queryDispatcher.DispatchAsync<GetInStockItemsQuery<IEnumerable<InventoryItemDto>>, IEnumerable<InventoryItemDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved in-stock inventory items successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No in-stock inventory items found");
                return NotFound("No in-stock inventory items found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetInStockItems: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockItems(int treshold)
        {
            try
            {
                var query = new GetLowStockItemsQuery<IEnumerable<InventoryItemDto>>() { Threshold = treshold};
                var result = await _queryDispatcher.DispatchAsync<GetLowStockItemsQuery<IEnumerable<InventoryItemDto>>, IEnumerable<InventoryItemDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved low-stock inventory items successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No low-stock inventory items found");
                return NotFound("No low-stock inventory items found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetLowStockItems: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("top-popular")]
        public async Task<IActionResult> GetTopPopularItems(int minScore, int maxScore)
        {
            try
            {
                var query = new GetTopPopularItemsQuery<IEnumerable<InventoryItemDto>>() { MinScore = minScore, MaxScore = maxScore};
                var result = await _queryDispatcher.DispatchAsync<GetTopPopularItemsQuery<IEnumerable<InventoryItemDto>>, IEnumerable<InventoryItemDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved top popular inventory items successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No top popular inventory items found");
                return NotFound("No top popular inventory items found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTopPopularItems: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchInventoryItems([FromQuery] string query)
        {
            try
            {
                var searchQuery = new SearchInventoryItemsQuery<IEnumerable<InventoryItemDto>> { Keyword = query };
                var result = await _queryDispatcher.DispatchAsync<SearchInventoryItemsQuery<IEnumerable<InventoryItemDto>>, IEnumerable<InventoryItemDto>>(searchQuery);

                if (result != null && result.Any())
                {
                    _logger.LogInformation($"Retrieved search results for '{query}' successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"No search results found for '{query}'");
                return NotFound($"No search results found for '{query}'");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SearchInventoryItems: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("validity-expiring-soon")]
        public async Task<IActionResult> GetInventoryItemsWithValidityExpiringSoon(DateTime expirationDate)
        {
            try
            {
                var query = new GetInventoryItemsWithValidityExpiringSoonQuery<IEnumerable<InventoryItemDto>>() { ExpirationDate = expirationDate };
                var result = await _queryDispatcher.DispatchAsync<GetInventoryItemsWithValidityExpiringSoonQuery<IEnumerable<InventoryItemDto>>, IEnumerable<InventoryItemDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved inventory items with validity expiring soon successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No inventory items with validity expiring soon found");
                return NotFound("No inventory items with validity expiring soon found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetInventoryItemsWithValidityExpiringSoon: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}