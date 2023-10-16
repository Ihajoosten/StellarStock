namespace StellarStock.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/suppliers")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class SupplierController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger<SupplierController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        /* :: Supplier Commands :: */
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierCommand command)
        {
            try
            {
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Supplier created successfully");
                return result == true ? Ok("Supplier created successfully") : UnprocessableEntity("Could not create Supplier");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateSupplier: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(string id, [FromBody] UpdateSupplierCommand command)
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
                _logger.LogInformation("Supplier updated successfully");
                return result == true ? Ok("Supplier updated successfully") : UnprocessableEntity("Could not update Supplier");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateSupplier: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            try
            {
                // Ensure the ID in the the route parameter is not null or empty
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogError("Invalid ID in the request");
                    return BadRequest("Invalid ID in the request");
                }

                var command = new DeleteSupplierCommand { Id = id };
                var result = await _commandDispatcher.DispatchAsync(command);
                _logger.LogInformation("Supplier deleted successfully");
                return result ? Ok("Supplier deleted successfully") : UnprocessableEntity("Could not delete Supplier");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteSupplier: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateSupplier(string id)
        {
            try
            {
                var command = new ActivateSupplierCommand { Id = id };
                var result = await _commandDispatcher.DispatchAsync(command);

                if (result)
                {
                    _logger.LogInformation("Supplier activated successfully");
                    return Ok("Supplier activated successfully");
                }
                else
                {
                    _logger.LogError("Could not activate Supplier");
                    return UnprocessableEntity("Could not activate Supplier");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ActivateSupplier: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateSupplier(string id)
        {
            try
            {
                var command = new DeactivateSupplierCommand { Id = id };
                var result = await _commandDispatcher.DispatchAsync(command);

                if (result)
                {
                    _logger.LogInformation("Supplier deactivated successfully");
                    return Ok("Supplier deactivated successfully");
                }
                else
                {
                    _logger.LogError("Could not deactivate Supplier");
                    return UnprocessableEntity("Could not deactivate Supplier");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeactivateSupplier: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /* :: Supplier Queries :: */
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveSuppliers()
        {
            try
            {
                var query = new GetActiveSuppliersQuery<IEnumerable<SupplierDto>>();
                var result = await _queryDispatcher.DispatchAsync<GetActiveSuppliersQuery<IEnumerable<SupplierDto>>, IEnumerable<SupplierDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved active suppliers successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No active suppliers found");
                return NotFound("No active suppliers found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetActiveSuppliers: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(string id)
        {
            try
            {
                var query = new GetSupplierByIdQuery<SupplierDto>() { Id = id };
                var result = await _queryDispatcher.DispatchAsync<GetSupplierByIdQuery<SupplierDto>, SupplierDto>(query);

                if (result != null)
                {
                    _logger.LogInformation($"Retrieved supplier with ID {id} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"Supplier with ID {id} not found");
                return NotFound($"Supplier with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSupplierById: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetSuppliersByCity(string city)
        {
            try
            {
                var query = new GetSuppliersByCityQuery<IEnumerable<SupplierDto>> { City = city };
                var result = await _queryDispatcher.DispatchAsync<GetSuppliersByCityQuery<IEnumerable<SupplierDto>>, IEnumerable<SupplierDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation($"Retrieved suppliers in {city} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"No suppliers in {city} found");
                return NotFound($"No suppliers in {city} found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSuppliersByCity: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("region/{region}")]
        public async Task<IActionResult> GetSuppliersByRegion(string region)
        {
            try
            {
                var query = new GetSuppliersByRegionQuery<IEnumerable<SupplierDto>> { Region = region };
                var result = await _queryDispatcher.DispatchAsync<GetSuppliersByRegionQuery<IEnumerable<SupplierDto>>, IEnumerable<SupplierDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation($"Retrieved suppliers in {region} successfully");
                    return Ok(result);
                }

                _logger.LogInformation($"No suppliers in {region} found");
                return NotFound($"No suppliers in {region} found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSuppliersByRegion: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("validity-expiring-soon")]
        public async Task<IActionResult> GetSuppliersWithValidityExpiringSoon(DateTime expirationDate)
        {
            try
            {
                var query = new GetSuppliersWithValidityExpiringSoonQuery<IEnumerable<SupplierDto>>() { ExpirationDate = expirationDate }; ;
                var result = await _queryDispatcher.DispatchAsync<GetSuppliersWithValidityExpiringSoonQuery<IEnumerable<SupplierDto>>, IEnumerable<SupplierDto>>(query);

                if (result != null && result.Any())
                {
                    _logger.LogInformation("Retrieved suppliers with validity expiring soon successfully");
                    return Ok(result);
                }

                _logger.LogInformation("No suppliers with validity expiring soon found");
                return NotFound("No suppliers with validity expiring soon found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSuppliersWithValidityExpiringSoon: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
