using CTodo.GraphQL.GraphQLTypes;
using Ctodo.Models.ViewModel;
using CTodo.Repositories.Infrastructure;
using GraphQL;
using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLMutations;

public class CategoryMutation : ObjectGraphType
{
    public CategoryMutation(ICategoryRepository repository)
    {
        Field<CategoryType>("newCategory")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }))
            .ResolveAsync(
                async context =>
                {
                    var categoryName = context.GetArgument<string>("name");

                    var category = new CategoryViewModel() { Name = categoryName };

                    return await repository.Create(category);
                });

        Field<CategoryType>("deleteCategory")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }))
            .ResolveAsync(
                async context =>
                {
                    var categoryId = context.GetArgument<Guid>("id");

                    return await repository.DeleteById(categoryId);
                });
    }
}