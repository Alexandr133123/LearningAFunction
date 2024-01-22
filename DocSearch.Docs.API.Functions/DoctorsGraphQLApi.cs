using HotChocolate.AzureFunctions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;

namespace DocSearch.Docs.API.Functions
{
    public class DoctorsGraphQLApi
    {
        private readonly IGraphQLRequestExecutor _executor;

        public DoctorsGraphQLApi(IGraphQLRequestExecutor executor)
        {
            _executor = executor;
        }

        [FunctionName("DoctorsGQL")]
        public async Task<IActionResult> GetDoctors(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "graphql/{**slug}")] HttpRequest request)
        {
            return await _executor.ExecuteAsync(request);
        }
    }
}
