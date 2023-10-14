using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

namespace StellarStock.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inventory-items")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class InventoryItemController : ControllerBase
    {
        private readonly ILogger<InventoryItemController> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public InventoryItemController(ILogger<InventoryItemController> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        // GET: api/inventory-items
        [HttpGet(Name = "GetInventoryItems")]
        public async Task<IEnumerable<string>> Get()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();

            return new string[] { "value1", "value2" };
        }

        // GET api/inventory-items/5
        [HttpGet("{itemId}")]
        public string Get(string itemId)
        {
            return "value";
        }

        // POST api/inventory-items
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/inventory-items/5
        [HttpPut("{itemId}")]
        public void Put(string itemId, [FromBody] string value)
        {
        }

        // DELETE api/inventory-items/5
        [HttpDelete("{itemId}")]
        public void Delete(string itemId)
        {
        }
    }
}
