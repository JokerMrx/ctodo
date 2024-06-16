import {IId} from "./IId.ts";
import {PriorityEnum} from "../enums/PriorityEnum.ts";
import {ICategory} from "./ICategory.ts";

export interface ITodo {
    TodoId: IId,
    Title: string,
    IsCompleted: boolean,
    Priority: PriorityEnum,
    DueDate: Date | null,
    Categories: ICategory[]
}