import {useSelector} from "react-redux";

import LayoutView from "../../layouts/layout-view.tsx";
import CategoryNew from "../../sections/categories/category-new.tsx";
import CategoriesList from "../../sections/categories/categories-list.tsx";
import {IState} from "../../interfaces/store.ts";

const CategoriesView = () => {
    const categories = useSelector((state: IState) => state.categories);

    return <LayoutView>
        <CategoryNew/>
        <CategoriesList categories={categories}/>
    </LayoutView>
};

export default CategoriesView;