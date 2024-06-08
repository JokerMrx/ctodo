using CTodo.GraphQL.GraphQLMutations;
using CTodo.GraphQL.GraphQLQueries;
using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLSchema;

public class AppSchema : Schema
{
    public AppSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = provider.GetRequiredService<AppQuery>();
        Mutation = provider.GetRequiredService<AppMutation>();
    }
}