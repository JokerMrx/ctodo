import {ITodo} from "./ITodo.ts";
import {IId} from "./IId.ts";
import {ICategory} from "./ICategory.ts";
import {DataStorageTypeEnum} from "../enums/DataStorageTypeEnum.ts";

export interface IState {
    todos: ITodo[];
    categories: ICategory[],
}

export interface IAction {
    type: string;
    payload: ITodo | IId | ITodo[] | ICategory[] | DataStorageTypeEnum,
}