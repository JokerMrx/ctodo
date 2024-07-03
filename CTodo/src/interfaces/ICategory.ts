import {IId} from "./IId.ts";

export interface ICategory {
    categoryId: IId,
    name: string
}

export type ICategoryCreate = Pick<ICategory, "name">;
