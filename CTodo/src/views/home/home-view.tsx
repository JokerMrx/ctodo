import {useEffect} from "react";
import {useDispatch, useSelector} from "react-redux";
import {useQuery} from "@apollo/client";

import LayoutView from "../../layouts/layout-view.tsx";
import TodoNew from "../../sections/todos/todo-new.tsx";
import TodoList from "../../sections/todos/todo-list.tsx";
import {GET_TODOS} from "../../services/api/todos.api.ts";
import {GET_CATEGORIES} from "../../services/api/categories.api.ts";
import {IState} from "../../interfaces/store.ts";

const HomeView = () => {
    const {loading: todosLoading, data: todosData} = useQuery(GET_TODOS);
    const {loading: categoriesLoading, data: categoriesData} = useQuery(GET_CATEGORIES);

    const dispatch = useDispatch();
    const categories = useSelector((state: IState) => state.categories);
    const todos = useSelector((state: IState) => state.todos);

    useEffect(() => {
        if (!todosData) return;

        dispatch({
            type: "SET_TODOS",
            payload: todosData.todos
        })
    }, [todosData]);

    useEffect(() => {
        if (!categoriesData) return;

        dispatch({
            type: "SET_CATEGORIES",
            payload: categoriesData.categories
        })
    }, [categoriesData]);

    return <LayoutView>
        {
            (todosLoading || categoriesLoading)
                ? <>Loading ...</>
                : <>
                    {
                        categories.length > 0 ? <TodoNew categories={categories}/> : <>Create the category first</>
                    }
                    <TodoList todos={todos}/>
                </>
        }
    </LayoutView>
};

export default HomeView;