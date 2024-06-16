import LayoutView from "../../layouts/layout-view.tsx";
import TodoNew from "../../sections/todos/todo-new.tsx";
import TodoList from "../../sections/todos/todo-list.tsx";

const HomeView = () => {
    return <LayoutView>
        <TodoNew/>
        <TodoList/>
    </LayoutView>
};

export default HomeView;