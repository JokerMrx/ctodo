import {useSelector} from "react-redux";

import {IState} from "../../interfaces/store.ts";
import TodoCard from "../../components/cards/todos/todo-card.tsx";

const TodoList = () => {
    const todos = useSelector((state: IState) => state.todos);

    return <div className="mt-5 w-100 d-flex flex-column align-items-center">
        {todos.map(todo => <TodoCard key={todo.TodoId} todo={todo}/>)}
    </div>
}

export default TodoList;