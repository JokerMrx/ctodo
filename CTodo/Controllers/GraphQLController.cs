using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using CTodo.GraphQL.GraphQLQueries;

namespace Ctodo.Controllers;

[Route("graphql")]
[ApiController]
public class GraphQLController : Controller
{
    private readonly IServiceProvider _serviceProvider;

    public GraphQLController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        using (var scope = _serviceProvider.CreateScope())
        {
            var schema = scope.ServiceProvider.GetRequiredService<ISchema>();
            var executer = scope.ServiceProvider.GetRequiredService<IDocumentExecuter>();

            var inputs = query.Variables != null ? new Inputs(query.Variables) : Inputs.Empty;
            var executionOptions = new ExecutionOptions
            {
                Schema = schema,
                Query = query.Query,
                Root = inputs
            };

            var result = await executer.ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}