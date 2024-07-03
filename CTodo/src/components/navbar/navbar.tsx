import {ChangeEvent} from "react";
import {Link} from "react-router-dom";

import {DATA_STORAGE_TYPE_KEY} from "../../constants";
import {DataStorageTypeEnum} from "../../enums/DataStorageTypeEnum.ts";

const Navbar = () => {
    const currentDataStorageType = localStorage.getItem(DATA_STORAGE_TYPE_KEY) ?? DataStorageTypeEnum.DATABASE;

    const handleChangeDataStorageType = (e: ChangeEvent<HTMLSelectElement>) => {
        const {value} = e.target;

        localStorage.setItem(DATA_STORAGE_TYPE_KEY, value);
    }

    return <header className="w-100">
        <nav
            className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div className="container-fluid">
                <a className="navbar-brand">CTodo</a>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse"
                        data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul className="navbar-nav flex-grow-1">
                        <li className="nav-item">
                            <Link className="nav-link text-dark" to="/">Home</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link text-dark" to="/">{"Todo"}</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link text-dark" to="/categories">Category</Link>
                        </li>
                    </ul>
                </div>
                <div className="d-flex gap-4">
                    <div>
                        <select className="form-select" onChange={handleChangeDataStorageType}>
                            <option value="database"
                                    selected={currentDataStorageType === DataStorageTypeEnum.DATABASE}>Database
                            </option>
                            <option value="xml" selected={currentDataStorageType === DataStorageTypeEnum.XML}>Xml
                                File
                            </option>
                        </select>
                    </div>
                    <div>
                        <button className="btn btn-outline-primary" type="submit">Change</button>
                    </div>
                </div>
            </div>
        </nav>
    </header>
};

export default Navbar;