using CTodo.GraphQL.GraphQLTypes;
using Ctodo.Models.ViewModel;
using CTodo.Repositories.Infrastructure;
using GraphQL;
using GraphQL.Types;

namespace CTodo.GraphQL.GraphQLMutations;

public class TodoMutation : ObjectGraphType
{
    public TodoMutation(ITodoRepository repository)
    {
        Field<TodoType>("newTodo")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "priority" },
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "category" },
                new QueryArgument<NonNullGraphType<DateGraphType>> { Name = "dueDate" }
            ))
            .ResolveAsync(async context =>
                {
                    var title = context.GetArgument<string>("title");
                    var priority = context.GetArgument<string>("priority");
                    var categoryId = context.GetArgument<Guid>("category");
                    var dueDate = context.GetArgument<DateTime>("dueDate");

                    var todo = new TodoItemViewModel()
                        { Title = title, Priority = priority, DueDate = dueDate, Category = categoryId };

                    return await repository.Create(todo);
                }
            );

        Field<TodoType>("toggleTodoCompleted")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<BooleanGraphType>> { Name = "isCompleted" }))
            .ResolveAsync(
                async context =>
                {
                    var todoId = context.GetArgument<Guid>("id");
                    var isCompleted = context.GetArgument<bool>("isCompleted");

                    return await repository.ToggleCompleted(todoId, isCompleted);
                });

        Field<TodoType>("deleteTodo")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                var todoId = context.GetArgument<Guid>("id");

                return await repository.DeleteById(todoId);
            });
    }
}