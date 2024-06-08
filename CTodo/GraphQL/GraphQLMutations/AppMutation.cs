using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLMutations;

public class AppMutation : ObjectGraphType
{
    public AppMutation(TodoMutation todoMutation, CategoryMutation categoryMutation)
    {
        AddField(todoMutation.GetField("newTodo"));
        AddField(todoMutation.GetField("toggleTodoCompleted"));
        AddField(todoMutation.GetField("deleteTodo"));
        AddField(categoryMutation.GetField("newCategory"));
        AddField(categoryMutation.GetField("deleteCategory"));
    }
}