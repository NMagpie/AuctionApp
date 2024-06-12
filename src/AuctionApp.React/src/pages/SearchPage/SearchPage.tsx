import { useLoaderData } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { hasMore, productDtoToProduct } from '../../common';
import { useApi } from '../../contexts/ApiContext';
import { ProductDto } from '../../api/openapi-generated';
import SearchResultCard from '../../components/Search/SearchResultCard';
import { ProductsApiSearchProductsRequest as QueryBody } from "../../api/openapi-generated";

import './SearchPage.css';
import SearchPanel from '../../components/Search/SearchPanel';

export default function SearchPage() {

    const { searchData, queryBody } = useLoaderData();

    const { api } = useApi();

    const [query, setQuery] = useState<QueryBody>(queryBody);

    const [searchResult, setSearchResult] = useState<Array<ProductDto>>(searchData.items);

    const [hasMoreData, setHasMoreData] = useState(hasMore(0, searchData.pageSize, searchData.total));

    useEffect(() => {
        setSearchResult(searchData.items);
    }, [searchData]);

    const fetchData = async () => {

        const nextQuery = {
            pageIndex: query.pageIndex + 1,
            ...query,
        };

        const { data } = await api.products.searchProducts(nextQuery);

        setSearchResult([
            ...searchResult,
            ...data.items
        ]);

        setQuery(nextQuery);

        setHasMoreData(hasMore(nextQuery.pageIndex, searchData.pageSize, data.total));
    };

    const firstProductIndex = () => Math.min(query.pageIndex * searchData.pageSize + 1, searchData.total);

    const lastProductIndex = () => Math.min((query.pageIndex + 1) * searchData.pageSize, searchData.total);

    return (
        <div className='search-page'>

            <h1 className='text-center md:text-start'>Search results: {query.searchQuery}</h1>

            <div className='search-page-body'>

                <SearchPanel query={query} setQuery={setQuery} />

                {searchResult.length != 0 ?

                    <div className='lg:w-3/4 w-10/12 mb-5'>
                        <h3>
                            Showed&nbsp;
                            {firstProductIndex()}&nbsp;-&nbsp;{lastProductIndex()}&nbsp;
                            Of {searchData.total} results
                        </h3>

                        <div className='search-product-result'>
                            {searchResult.map(product => <SearchResultCard key={product.id} product={productDtoToProduct(product)} />)}
                        </div>
                    </div>

                    :
                    <h1 className='lg:w-3/4 text-center mb-5'>Sorry, nothing's here!</h1>
                }

            </div>
        </div>
    );
}