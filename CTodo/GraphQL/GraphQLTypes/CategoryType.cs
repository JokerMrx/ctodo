using Ctodo.Models;
using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLTypes;

public sealed class CategoryType : ObjectGraphType<Category>
{
    public CategoryType()
    {
        Field(x => x.CategoryId, type: typeof(IdGraphType)).Description("Category ID.");
        Field(x => x.Name).Description("Category name.");
    }
}