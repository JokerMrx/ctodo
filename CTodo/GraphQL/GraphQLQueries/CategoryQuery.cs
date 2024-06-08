using CTodo.GraphQL.GraphQLTypes;
using CTodo.Repositories.Infrastructure;
using GraphQL;
using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLQueries;

public class CategoryQuery : ObjectGraphType
{
    public CategoryQuery(ICategoryRepository repository)
    {
        Field<ListGraphType<CategoryType>>(
            "categories"
        ).ResolveAsync(async _ => await repository.Categories());

        Field<CategoryType>("category")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                var categoryId = context.GetArgument<Guid>("id");

                return await repository.GetById(categoryId);
            });
    }
}