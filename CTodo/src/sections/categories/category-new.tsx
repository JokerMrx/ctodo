import {ChangeEvent, useState, useEffect, useRef} from "react";
import {useDispatch} from "react-redux";
import {useLazyQuery, useMutation} from "@apollo/client";
import {fromEvent} from "rxjs";

import {CREATE_CATEGORY, GET_CATEGORIES} from "../../services/api/categories.api.ts";

const CategoryNew = () => {
    const [formData, setFormData] = useState({
        name: ""
    });
    const [newCategory] = useMutation(CREATE_CATEGORY);
    const [getCategories] = useLazyQuery(GET_CATEGORIES, {
        fetchPolicy: "network-only"
    });

    const dispatch = useDispatch();

    const buttonSubmitRef = useRef<HTMLButtonElement>(null);
    const formDataRef = useRef(formData);

    useEffect(() => {
        if (!buttonSubmitRef.current) return;

        const click = fromEvent(buttonSubmitRef.current, "click").subscribe(
            async () => {
                const currentFormData = formDataRef.current;

                await newCategory({
                    variables: {
                        categoryName: currentFormData.name
                    }
                });

                const {data} = await getCategories();

                dispatch({
                    type: "SET_CATEGORIES",
                    payload: data.categories
                });
            }
        );

        return () => click.unsubscribe();
    }, []);

    useEffect(() => {
        formDataRef.current = formData;
    }, [formData]);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;

        setFormData({...formData, [name]: value});
    }

    return <div className="mt-5 w-100">
        <h2 className="text-center">Create category</h2>
        <div className="d-flex justify-content-center">
            <form className="mt-2 w-100" style={{maxWidth: 450}}>
                <div className="my-3">
                    <label htmlFor="Name" className="form-label">Category name</label>
                    <input type="text" className="form-control" name="name" onChange={handleChange}/>
                </div>
                <div className="mt-3 d-flex justify-content-center">
                    <button className="btn btn-outline-success" type="button" ref={buttonSubmitRef}>Send</button>
                </div>
            </form>
        </div>
    </div>
};

export default CategoryNew;