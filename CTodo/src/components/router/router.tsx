import {
    createBrowserRouter
} from "react-router-dom";

import HomeView from "../../views/home/home-view.tsx";
import CategoriesView from "../../views/categories/categories-view.tsx";

export const router = createBrowserRouter([
    {
        path: "/",
        element: <HomeView/>,
    },
    {
        path: "/categories",
        element: <CategoriesView/>,
    },
]);