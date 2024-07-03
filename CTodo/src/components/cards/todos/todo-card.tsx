import {FC, useEffect, useRef} from "react";
import {fromEvent, mergeMap, Subscription} from "rxjs";
import {useLazyQuery, useMutation} from "@apollo/client";

import {ITodo} from "../../../interfaces/ITodo.ts";
import {DELETE_TODO_BY_ID, GET_TODOS, TOGGLE_TODO_COMPLETED} from "../../../services/api/todos.api.ts";
import {useDispatch} from "react-redux";

interface IProps {
    todo: ITodo
}

const TodoCard: FC<IProps> = ({todo}) => {
    const [deleteTodo] = useMutation(DELETE_TODO_BY_ID);
    const [toggleTodoCompleted] = useMutation(TOGGLE_TODO_COMPLETED);
    const [getTodos] = useLazyQuery(GET_TODOS, {
        fetchPolicy: "network-only"
    });

    const deleteButtonRef = useRef<HTMLButtonElement>(null);
    const toggleCheckboxRef = useRef<HTMLInputElement>(null);

    const dispatch = useDispatch();

    useEffect(() => {
        const subscriptions: Subscription[] = [];

        if (deleteButtonRef.current) {
            const deleteClick$ = fromEvent(deleteButtonRef.current, 'click').pipe(
                mergeMap(async () => {
                    await deleteTodo({
                        variables: {
                            todoId: todo.todoId
                        }
                    });

                    const {data} = await getTodos();
                    console.log("delete", data);
                    dispatch({
                        type: "SET_TODOS",
                        payload: data.todos
                    });
                })
            );
            subscriptions.push(deleteClick$.subscribe());
        }

        if (toggleCheckboxRef.current) {
            const toggleClick$ = fromEvent(toggleCheckboxRef.current, 'click').pipe(
                mergeMap(async () => {
                    await toggleTodoCompleted({
                        variables: {
                            todoId: todo.todoId,
                            isCompleted: !todo.isCompleted
                        }
                    });

                    const {data} = await getTodos();
                    console.log("delete", data);
                    dispatch({
                        type: "SET_TODOS",
                        payload: data.todos
                    });
                })
            );
            subscriptions.push(toggleClick$.subscribe());
        }

        return () => {
            subscriptions.forEach(subscription => subscription.unsubscribe());
        };
    }, [deleteTodo, toggleTodoCompleted, todo.todoId, todo.isCompleted]);

    return <div className="my-3 mx-1 col" style={{maxWidth: "600px", width: "100%"}}>
        <div className="card d-flex flex-row">
            <div className="card-body">
                <h5 className="card-title">{todo.title}</h5>
                <p className="card-text">Priority: {todo.priority}</p>
                {
                    todo.dueDate && <p className="card-text">Due Date: {new Date(todo.dueDate).toLocaleDateString()}</p>
                }
                <div className="form-check">
                    <div id="formToggleTodoCompleted-@todo.TodoId">
                        <input className="form-check-input" type="checkbox" name="IsCompleted"
                               checked={todo.isCompleted}
                               ref={toggleCheckboxRef}
                        />
                    </div>
                    <label className={`form-check-label ${todo.isCompleted ? " text-success" : " text-warning"}`}
                    >
                        Completed
                    </label>
                </div>
                <div className="mt-2">
                    {todo.categories?.map(category =>
                        <p className="lead m-0 text-primary" key={category.categoryId}>Category: {category.name}</p>
                    )}
                </div>
            </div>
            <div className="p-2 d-flex justify-content-end">
                <div>
                    <button className="btn btn-outline-danger" type="submit" name="TodoId"
                            ref={deleteButtonRef}>Delete
                    </button>
                </div>
            </div>
        </div>
    </div>
}

export default TodoCard;