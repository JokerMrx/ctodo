using Ctodo.Models;
using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLTypes;

public sealed class TodoType : ObjectGraphType<Todo>
{
    public TodoType()
    {
        Field(x => x.TodoId, type: typeof(IdGraphType)).Description("The ID of the Todo.");
        Field(x => x.Title).Description("The title of the Todo.");
        Field(x => x.IsCompleted).Description("The completion status of the Todo.");
        Field(x => x.Priority).Description("The priority of the Todo.");
        Field(x => x.DueDate, nullable: true).Description("The due date of the Todo.");
        Field<ListGraphType<CategoryType>>("categories").Description("The categories of the Todo."); 
    }
}