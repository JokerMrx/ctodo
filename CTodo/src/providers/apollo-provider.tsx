import {FC} from "react";
import {ApolloClient, InMemoryCache, ApolloProvider} from '@apollo/client';

import {IChildren} from "../interfaces/IChildren.ts";
import {API_URL, DATA_STORAGE_TYPE_KEY} from "../constants";
import {DataStorageTypeEnum} from "../enums/DataStorageTypeEnum.ts";

const client = new ApolloClient({
    uri: API_URL,
    cache: new InMemoryCache(),
    headers: {
        "Database-type": localStorage.getItem(DATA_STORAGE_TYPE_KEY) ?? DataStorageTypeEnum.DATABASE
    }
});
const ApolloAppProvider: FC<IChildren> = ({children}) => {
    return <ApolloProvider client={client}>{children}</ApolloProvider>;
};
export default ApolloAppProvider;