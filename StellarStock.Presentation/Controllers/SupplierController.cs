using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

namespace StellarStock.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/suppliers")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public SupplierController(ILogger<SupplierController> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        // GET: api/suppliers
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();

            return new string[] { "value1", "value2" };
        }

        // GET api/suppliers/5
        [HttpGet("{supplierId}")]
        public string Get(string supplierId)
        {
            return "value";
        }

        // POST api/suppliers
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/suppliers/5
        [HttpPut("{supplierId}")]
        public void Put(string supplierId, [FromBody] string value)
        {
        }

        // DELETE api/suppliers/5
        [HttpDelete("{supplierId}")]
        public void Delete(string supplierId)
        {
        }
    }
}
