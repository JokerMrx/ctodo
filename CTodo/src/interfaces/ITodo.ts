import {IId} from "./IId.ts";
import {PriorityEnum} from "../enums/PriorityEnum.ts";
import {ICategory} from "./ICategory.ts";

export interface ITodo {
    todoId: IId,
    title: string,
    isCompleted: boolean,
    priority: PriorityEnum,
    dueDate: Date | null,
    categories: ICategory[]
}

export type ITodoCreate = Pick<ITodo, "title" | "priority" | "dueDate"> & {
    category: IId
};