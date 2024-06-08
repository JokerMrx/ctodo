using CTodo.GraphQL.GraphQLTypes;
using CTodo.Repositories.Infrastructure;
using GraphQL;
using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLQueries;

public class TodoQuery : ObjectGraphType
{
    public TodoQuery(ITodoRepository repository)
    {
        Field<ListGraphType<TodoType>>("todos")
            .ResolveAsync(async _ => await repository.Todos()
            );

        Field<TodoType>(
            "todo"
        ).Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" })).ResolveAsync(
            async context =>
            {
                var id = context.GetArgument<Guid>("id");

                return await repository.GetById(id);
            });
    }
}