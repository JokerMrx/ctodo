import {ITodo} from "../interfaces/ITodo.ts";
import {IId} from "../interfaces/IId.ts";

export const isValidateTodo = (todo: any): todo is ITodo => {
    return (
        todo &&
        typeof todo === "object" &&
        (typeof todo.TodoId === "number" || typeof todo.TodoId === "string") &&
        typeof todo.Title === "string" &&
        typeof todo.IsCompleted === "boolean" &&
        typeof todo.Priority === "string"
    );
};

export const isTodoStatusUpdate = (payload: any): payload is { todoId: IId, isCompleted: boolean } => {
    return (payload as { todoId: IId, isCompleted: boolean }).todoId !== undefined && (payload as { todoId: IId, isCompleted: boolean }).isCompleted !== undefined;
};