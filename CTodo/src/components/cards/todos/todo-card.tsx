import {useDispatch} from "react-redux";

import {ITodo} from "../../../interfaces/ITodo.ts";
import {IId} from "../../../interfaces/IId.ts";

const TodoCard = ({todo}: { todo: ITodo }) => {
    const dispatch = useDispatch();

    const handleDeleteTodo = (todoId: IId) => {
        dispatch({
            type: "DELETE_TODO",
            payload: todoId
        });
    }

    const handleChangeTodoCompleted = (todoId: IId, isCompleted: boolean) => {
        dispatch({
            type: "TOGGLE_COMPLETED_TODO",
            payload: {
                todoId,
                isCompleted: !isCompleted
            }
        });
    }

    return <div className="my-3 mx-1 col" style={{maxWidth: "600px", width: "100%"}}>
        <div className="card d-flex flex-row">
            <div className="card-body">
                <h5 className="card-title">{todo.Title}</h5>
                <p className="card-text">Priority: {todo.Priority}</p>
                {
                    todo.DueDate && <p className="card-text">Due Date: {new Date(todo.DueDate).toLocaleDateString()}</p>
                }
                <div className="form-check">
                    <div id="formToggleTodoCompleted-@todo.TodoId">
                        <input className="form-check-input" type="checkbox" name="IsCompleted"
                               checked={todo.IsCompleted}
                               onClick={() => handleChangeTodoCompleted(todo.TodoId, todo.IsCompleted)}
                        />
                    </div>
                    <label className={`form-check-label ${todo.IsCompleted ? " text-success" : " text-warning"}`}
                    >
                        Completed
                    </label>
                </div>
                <div className="mt-2">
                    {todo.Categories?.map(category =>
                        <p className="lead m-0 text-primary" key={category.CategoryId}>Category: {category.Name}</p>
                    )}
                </div>
            </div>
            <div className="p-2 d-flex justify-content-end">
                <div>
                    <button className="btn btn-outline-danger" type="submit" name="TodoId"
                            onClick={() => handleDeleteTodo(todo.TodoId)}>Delete
                    </button>
                </div>
            </div>
        </div>
    </div>
}

export default TodoCard;