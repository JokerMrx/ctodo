import {FC} from "react";

import Navbar from "../components/navbar/navbar.tsx";

import {IChildren} from "../interfaces/IChildren.ts";

const LayoutView: FC<IChildren> = ({children}) => {
    return <main className="d-flex flex-column align-items-center">
        <Navbar/>
        <>
            {children}
        </>
    </main>
}

export default LayoutView;