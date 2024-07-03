import {FC, useEffect, useRef} from "react";
import {useDispatch} from "react-redux";
import {useLazyQuery, useMutation} from "@apollo/client";
import {fromEvent, mergeMap, Subscription} from "rxjs";

import {ICategory} from "../../interfaces/ICategory";
import {DELETE_CATEGORY_BY_ID, GET_CATEGORIES} from "../../services/api/categories.api.ts";

interface IProps {
    categories: ICategory[];
}

const CategoriesList: FC<IProps> = ({categories}) => {
    const [deleteCategory] = useMutation(DELETE_CATEGORY_BY_ID);
    const [getCategories] = useLazyQuery(GET_CATEGORIES, {
        fetchPolicy: "network-only"
    });

    const dispatch = useDispatch();

    const buttonDeleteRefs = useRef(new Map<string, HTMLButtonElement>(null));

    useEffect(() => {
        const subscriptions: Subscription[] = [];

        categories.forEach(({categoryId}) => {
            const button = buttonDeleteRefs.current.get(categoryId.toString());
            if (button) {
                const click$ = fromEvent(button, 'click').pipe(
                    mergeMap(async () => {
                        await deleteCategory({
                            variables: {categoryId}
                        });

                        const {data} = await getCategories();

                        dispatch({
                            type: "SET_CATEGORIES",
                            payload: data.categories
                        })
                    })
                );

                subscriptions.push(click$.subscribe());
            }
        });

        return () => {
            subscriptions.forEach(subscription => subscription.unsubscribe());
        };
    }, [categories, deleteCategory]);

    return <div className="w-100">
        <h2 className="mt-4 text-center">Categories</h2>
        <div className="mt-3">
            <div className="d-flex justify-content-center">
                <div className="mt-3" style={{maxWidth: "450px", width: "100%"}}>
                    {
                        categories.length > 0
                            ? categories.map(({categoryId, name}) => <div
                                key={categoryId}
                                className="my-2 px-2 py-1 d-flex justify-content-between border border-1 border-secondary rounded">
                                <div>
                                    {name}
                                </div>
                                <div>
                                    <div>
                                        <button className="btn btn-outline-danger" type="button"
                                                ref={ref => {
                                                    if (ref) buttonDeleteRefs.current.set(categoryId.toString(), ref);
                                                }}
                                        >Delete
                                        </button>
                                    </div>
                                </div>
                            </div>)
                            : <div>
                                <h3 className="text-center">No categories created yet</h3>
                            </div>
                    }
                </div>
            </div>
        </div>
    </div>
};

export default CategoriesList;