using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLQueries;

public class AppQuery : ObjectGraphType
{
    public AppQuery(TodoQuery todoQuery, CategoryQuery categoryQuery)
    {
        AddField(todoQuery.GetField("todos"));
        AddField(todoQuery.GetField("todo"));
        AddField(categoryQuery.GetField("categories"));
        AddField(categoryQuery.GetField("category"));
    }
}