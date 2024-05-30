const btnCreateTodo = document.querySelector("#create-todo");
const inpTodoTitle = document.querySelector("#Title");

const handleToggleTodoCompleted = (todoId) => {
    document.querySelector(`#formToggleTodoCompleted-${todoId}`).submit();
}

inpTodoTitle.addEventListener('input', () => {
    btnCreateTodo.disabled = inpTodoTitle.value.trim() === '';
});
