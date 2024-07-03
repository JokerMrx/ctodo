import {ChangeEvent, FC, useState, useEffect, useRef} from "react";
import {useMutation, useLazyQuery} from "@apollo/client";
import {fromEvent} from "rxjs";

import {PriorityEnum} from "../../enums/PriorityEnum.ts";
import PRIORITY from "../../data/priority.data.json";
import {ICategory} from "../../interfaces/ICategory.ts";
import {CREATE_TODO, GET_TODOS} from "../../services/api/todos.api.ts";

interface IProps {
    categories: ICategory[];
}

const TodoNew: FC<IProps> = ({categories}) => {
    const [formData, setFormData] = useState({
        title: "",
        category: categories[0].categoryId,
        priority: PRIORITY[0],
        dueDate: ""
    });
    const [newTodo] = useMutation(CREATE_TODO);
    const [getTodos] = useLazyQuery(GET_TODOS, {
        fetchPolicy: "network-only"
    });

    const buttonSubmitRef = useRef<HTMLButtonElement>(null);
    const formDataRef = useRef(formData);

    useEffect(() => {
        if (!buttonSubmitRef.current) return;

        const click = fromEvent(buttonSubmitRef.current, "click").subscribe(async () => {
            const _categories = categories.filter(({categoryId}) => categoryId === formData.category);
            const currentFormData = formDataRef.current;

            await newTodo({
                variables: {
                    title: currentFormData.title,
                    priority: currentFormData.priority as PriorityEnum,
                    dueDate: !!currentFormData.dueDate ? new Date(currentFormData.dueDate).toISOString() : null,
                    categoryId: _categories[0].categoryId
                }
            });

            const {data: dt} = await getTodos();

            console.log({dt})
        })

        return () => click.unsubscribe();
    }, []);

    useEffect(() => {
        formDataRef.current = formData;
    }, [formData]);

    const handleChange = (e: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLSelectElement>) => {
        const {name, value} = e.target;

        setFormData({...formData, [name]: value});
    }

    return <div className="d-flex justify-content-center">
        <div className="mt-3 w-100 row" style={{maxWidth: 450}}>
            <h2 className="mb-3 text-center">{"New Todo"}</h2>
            <div className="col">
                <div className="mb-3">
                    <label htmlFor="title" className="form-label">Title</label>
                    <input type="text" className="form-control" name="title" value={formData.title}
                           onChange={handleChange}/>
                    <span className="text-danger"></span>
                </div>
                <div className="mb-3">
                    <label htmlFor="category" className="form-label">Category</label>
                    <select className="form-select" name="category" onChange={handleChange}>
                        {
                            categories.map(({categoryId, name}) => <option key={categoryId} value={categoryId}>
                                {name}
                            </option>)
                        }
                    </select>
                    <span className="text-danger"></span>
                </div>
            </div>
            <div className="col">
                <div className="mb-3">
                    <label htmlFor="priority" className="form-label">Priority</label>
                    <select className="form-select" name="priority" onChange={handleChange}>
                        {
                            PRIORITY.map((item, index) => <option key={index} value={item}>{item}</option>)
                        }
                    </select>
                    <span className="text-danger"></span>
                </div>
                <div className="mb-3">
                    <label htmlFor="dueDate" className="form-label">Due date</label>
                    <input type="date" className="form-control" name="dueDate" value={formData.dueDate}
                           onChange={handleChange}/>
                    <span className="text-danger"></span>
                </div>
            </div>
            <div className="d-flex justify-content-end">
                <button id="create-todo" className="btn btn-outline-success" ref={buttonSubmitRef}>Send</button>
            </div>
        </div>
    </div>
};

export default TodoNew;