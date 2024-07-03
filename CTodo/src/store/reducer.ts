import {IState, IAction} from '../interfaces/store.ts';
import {ITodo} from "../interfaces/ITodo.ts";
import {ICategory} from "../interfaces/ICategory.ts";

const initialState: IState = {
    todos: [],
    categories: [],
};

const reducer = (state: IState = initialState, action: IAction): IState => {
        switch (action.type) {
            case "SET_TODOS": {
                const todos: ITodo[] = action.payload as ITodo[];

                return {...state, todos};
            }
            case "SET_CATEGORIES": {
                const categories: ICategory[] = action.payload as ICategory[];

                return {...state, categories};
            }
            default:
                return state;
        }
    }
;

export default reducer;