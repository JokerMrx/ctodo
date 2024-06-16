import {ChangeEvent, useState} from "react";
import {useDispatch} from "react-redux";

import {ITodo} from "../../interfaces/ITodo.ts";
import {PriorityEnum} from "../../enums/PriorityEnum.ts";
import CATEGORIES from "../../data/categories.data.json";
import PRIORITY from "../../data/priority.data.json";

const TodoNew = () => {
    const [formData, setFormData] = useState({
        title: "",
        category: CATEGORIES[0].CategoryId,
        priority: PRIORITY[0],
        dueDate: ""
    });
    const dispatch = useDispatch();

    const handleChange = (e: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLSelectElement>) => {
        const {name, value} = e.target;

        setFormData({...formData, [name]: value});
    }

    const onSubmit = () => {
        const newTodo: ITodo = {
            TodoId: crypto.randomUUID(),
            Title: formData.title,
            IsCompleted: false,
            Priority: formData.priority as PriorityEnum,
            DueDate: !!formData.dueDate ? new Date(formData.dueDate) : null,
            Categories: CATEGORIES.filter(({CategoryId}) => CategoryId === formData.category)
        };

        console.log({dispatch, formData, newTodo});
        dispatch({
            type: "NEW_TODO",
            payload: newTodo
        });
    };

    return <div className="d-flex justify-content-center">
        <div className="mt-3 row" style={{maxWidth: "450px", width: "100%"}}>
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
                            CATEGORIES.map(({CategoryId, Name}) => <option value={CategoryId}>
                                {Name}
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
                            PRIORITY.map(item => <option value={item}>{item}</option>)
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
                <button id="create-todo" className="btn btn-outline-success" onClick={onSubmit}>Send</button>
            </div>
        </div>
    </div>
};

export default TodoNew;