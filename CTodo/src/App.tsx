import {Provider} from "react-redux";
import {RouterProvider} from "react-router-dom";

import ApolloAppProvider from "./providers/apollo-provider.tsx";
import {router} from "./components/router/router.tsx";
import store from "./store/store.ts";

import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    return (
        <ApolloAppProvider>
            <Provider store={store}>
                <RouterProvider router={router}/>
            </Provider>
        </ApolloAppProvider>
    )
}

export default App
