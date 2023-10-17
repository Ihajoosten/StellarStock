namespace StellarStock.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/warehouses")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WarehouseController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ILogger<WarehouseController> _logger;

        public WarehouseController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger<WarehouseController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        /* :: Warehouse Commands :: */
        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseCommand command)
        {
            try
            {
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Warehouse created successfully");
                return result == true ? Ok("Warehouse created successfully") : UnprocessableEntity("Could not create Warehouse");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateWarehouse: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(string id, [FromBody] UpdateWarehouseCommand command)
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
                _logger.LogInformation("Warehouse updated successfully");
                return result == true ? Ok("Warehouse updated successfully") : UnprocessableEntity("Could not update Warehouse");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateWarehouse: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(string id)
        {
            try
            {
                // Ensure the ID in the route parameter is not null or empty
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogError("Invalid ID in the request");
                    return BadRequest("Invalid ID in the request");
                }

                var command = new DeleteWarehouseCommand { Id = id };
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Warehouse deleted successfully");
                return result ? Ok("Warehouse deleted successfully") : UnprocessableEntity("Could not delete Warehouse");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteWarehouse: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseWarehouse(string id)
        {
            try
            {
                var command = new CloseWarehouseCommand { Id = id };
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Warehouse closed successfully");
                return result ? Ok("Warehouse closed successfully") : UnprocessableEntity("Could not close Warehouse");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CloseWarehouse: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/reopen")]
        public async Task<IActionResult> ReopenWarehouse(string id)
        {
            try
            {
                var command = new ReopenWarehouseCommand { Id = id };
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Warehouse reopened successfully");
                return result ? Ok("Warehouse reopened successfully") : UnprocessableEntity("Could not reopen Warehouse");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReopenWarehouse: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/move")]
        public async Task<IActionResult> MoveWarehouse(string id, [FromBody] MoveWarehouseCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    _logger.LogError("Invalid ID in the request");
                    return BadRequest("Invalid ID in the request");
                }

                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Warehouse moved successfully");
                return result ? Ok("Warehouse moved successfully") : UnprocessableEntity("Could not move Warehouse");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MoveWarehouse: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /* :: Warehouse Queries :: */
        [HttpGet("closed")]
        public async Task<IActionResult> GetClosedWarehouses()
        {
            try
            {
                var query = new GetClosedWarehousesQuery<IEnumerable<WarehouseDto>>();
                var result = await _queryDispatcher.DispatchAsync<GetClosedWarehousesQuery<IEnumerable<WarehouseDto>>, IEnumerable<WarehouseDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved closed warehouses successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No closed warehouses found");
                return NotFound("No closed warehouses found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetClosedWarehouses: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("opened")]
        public async Task<IActionResult> GetOpenedWarehouses()
        {
            try
            {
                var query = new GetOpenedWarehousesQuery<IEnumerable<WarehouseDto>>();
                var result = await _queryDispatcher.DispatchAsync<GetOpenedWarehousesQuery<IEnumerable<WarehouseDto>>, IEnumerable<WarehouseDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved opened warehouses successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No opened warehouses found");
                return NotFound("No opened warehouses found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOpenedWarehouses: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseById(string id)
        {
            try
            {
                var query = new GetWarehouseByIdQuery<WarehouseDto>() { Id = id };
                var result = await _queryDispatcher.DispatchAsync<GetWarehouseByIdQuery<WarehouseDto>, WarehouseDto>(query);

                if (result != null)
                {
                    _logger.LogInformation($"Retrieved warehouse with ID {id} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"Warehouse with ID {id} not found");
                return NotFound($"Warehouse with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWarehouseById: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}/stocked-items")]
        public async Task<IActionResult> GetWarehouseStockedItems(string id)
        {
            try
            {
                var query = new GetWarehouseStockedItemsQuery<IEnumerable<WarehouseDto>> { Id = id };
                var result = await _queryDispatcher.DispatchAsync<GetWarehouseStockedItemsQuery<IEnumerable<WarehouseDto>>, IEnumerable<WarehouseDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation($"Retrieved stocked items for warehouse with ID {id} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"No stocked items found for warehouse with ID {id}");
                return NotFound($"No stocked items found for warehouse with ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWarehouseStockedItems: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetWarehousesByCity(string city)
        {
            try
            {
                var query = new GetWarehousesByCityQuery<IEnumerable<WarehouseDto>> { City = city };
                var result = await _queryDispatcher.DispatchAsync<GetWarehousesByCityQuery<IEnumerable<WarehouseDto>>, IEnumerable<WarehouseDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation($"Retrieved warehouses in {city} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"No warehouses in {city} found");
                return NotFound($"No warehouses in {city} found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWarehousesByCity: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("region/{region}")]
        public async Task<IActionResult> GetWarehousesByRegion(string region)
        {
            try
            {
                var query = new GetWarehousesByRegionQuery<IEnumerable<WarehouseDto>> { Region = region };
                var result = await _queryDispatcher.DispatchAsync<GetWarehousesByRegionQuery<IEnumerable<WarehouseDto>>, IEnumerable<WarehouseDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation($"Retrieved warehouses in {region} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"No warehouses in {region} found");
                return NotFound($"No warehouses in {region} found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWarehousesByRegion: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
