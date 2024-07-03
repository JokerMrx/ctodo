import TodoCard from "../../components/cards/todos/todo-card.tsx";
import {ITodo} from "../../interfaces/ITodo.ts";
import {FC} from "react";

interface IProps {
    todos: ITodo[]
}

const TodoList: FC<IProps> = ({todos}) => {
    return <div className="mt-5 w-100 d-flex flex-column align-items-center">
        {todos.map(todo => <TodoCard key={todo.todoId} todo={todo}/>)}
    </div>
}

export default TodoList;