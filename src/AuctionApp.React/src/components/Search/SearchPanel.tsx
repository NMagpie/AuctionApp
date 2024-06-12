import { Dispatch, SetStateAction } from 'react'
import { ProductsApiSearchProductsRequest as QueryBody } from "../../api/openapi-generated";

import './SearchPanel.css';

type Props = {
    query: QueryBody,
    setQuery: Dispatch<SetStateAction<QueryBody>>,
};

function SearchPanel({ query, setQuery }: Props) {
    return (
        <div className='search-page-panel'>
            Hola! ¿Como estas?
        </div>
    )
}

export default SearchPanel;