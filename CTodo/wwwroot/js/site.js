const btnCreateTodo = document.querySelector("#create-todo");
const inpTodoTitle = document.querySelector("#Title");

const handleToogleTodoCompleted = (todoId) => {
    document.querySelector(`#formToogleTodoCompleted-${todoId}`).submit();
}

inpTodoTitle.addEventListener('input', () => {
    btnCreateTodo.disabled = inpTodoTitle.value.trim() === '';
});
