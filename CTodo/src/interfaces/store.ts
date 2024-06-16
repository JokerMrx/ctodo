import {ITodo} from "./ITodo.ts";
import {IId} from "./IId.ts";

export interface IState {
    todos: ITodo[];
}

export interface IAction {
    type: string;
    payload: ITodo | IId | {
        todoId: IId,
        isCompleted: boolean
    },
}