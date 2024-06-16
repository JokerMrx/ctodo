import {IState, IAction} from '../interfaces/store.ts';
import {isTodoStatusUpdate, isValidateTodo} from "../utils/validate.ts";

const initialState: IState = {
    todos: [],
};

const reducer = (state: IState = initialState, action: IAction): IState => {
        switch (action.type) {
            case "NEW_TODO": {
                const todo = action.payload;
                if (isValidateTodo(todo)) {
                    return {
                        ...state,
                        todos: [...state.todos, todo],
                    };
                }

                return state;
            }
            case
            "TOGGLE_COMPLETED_TODO"
            : {
                if(!isTodoStatusUpdate(action.payload)) return state;
                
                const {todoId, isCompleted}= action.payload;
                const todos = state.todos.map(todo => {
                    if(todo.TodoId === todoId){
                        todo.IsCompleted = isCompleted;
                        return todo;
                    }
                    
                    return todo;
                })

                return {
                    ...state,
                    todos,
                };
            }
            case
            "DELETE_TODO"
            : {
                const todoId = action.payload;
                const todos = state.todos.filter(todo => todo.TodoId !== todoId);
                
                return {...state, todos};
            }
            default:
                return state;
        }
    }
;

export default reducer;