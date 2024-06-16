const Navbar = () => {
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
                            <a className="nav-link text-dark" href="#">Home</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link text-dark" href="#">{"Todo"}</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link text-dark" href="#">Category</a>
                        </li>
                    </ul>
                </div>
                <div className="d-flex gap-4">
                    <div>
                        <select className="form-select" name="StorageType" id="Storage">
                            <option value="database">Database</option>
                            <option value="xml">Xml File</option>
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