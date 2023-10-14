using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

namespace StellarStock.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/warehouses")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public WarehouseController(ILogger<WarehouseController> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        // GET: api/api/warehouses
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();

            return new string[] { "value1", "value2" };
        }

        // GET api/warehouses/5
        [HttpGet("{warehouseId}")]
        public string Get(string warehouseId)
        {
            return "value";
        }

        // POST api/warehouses
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUTapi/warehouses/5
        [HttpPut("{warehouseId}")]
        public void Put(string warehouseId, [FromBody] string value)
        {
        }

        // DELETE api/warehouses/5
        [HttpDelete("{warehouseId}")]
        public void Delete(string warehouseId)
        {
        }
    }
}
