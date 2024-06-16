import {Provider} from "react-redux";

import HomeView from "./views/home/home-view.tsx";
import store from "./store/store.ts";

import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    return (
        <Provider store={store}>
            <HomeView/>
        </Provider>
    )
}

export default App
