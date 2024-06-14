import { useLoaderData, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { getSearchQuery, productDtoToProduct } from '../../common';
import { ProductDto } from '../../api/openapi-generated';
import SearchResultCard from '../../components/Search/SearchResultCard';
import SearchPanel from '../../components/Search/SearchPanel';
import { Pagination } from '@mui/material';
import { ProductsApiSearchProductsRequest as QueryBody } from "../../api/openapi-generated";

import './SearchPage.css';

export default function SearchPage() {

    const { searchData, queryBody } = useLoaderData();

    const [searchResult, setSearchResult] = useState<Array<ProductDto>>(searchData.items);

    const navigate = useNavigate();

    const navigateToPage = (_, value: number) => {

        const modifiedQuery: QueryBody = {
            ...queryBody,
            pageIndex: value - 1,
        };

        const searchQueryString = getSearchQuery(modifiedQuery);

        navigate(searchQueryString);
    }

    useEffect(() => {
        setSearchResult(searchData.items);
    }, [searchData]);

    const firstProductIndex = () => Math.min(queryBody.pageIndex * searchData.pageSize + 1, searchData.total);

    const lastProductIndex = () => Math.min((queryBody.pageIndex + 1) * searchData.pageSize, searchData.total);

    const searchTitle = () => {
        if (!queryBody.searchQuery) {
            return "";
        }

        return queryBody.searchQuery.length < 30 ?
            queryBody.searchQuery :
            `${queryBody.searchQuery.substring(0, 25)}...`;
    }

    return (
        <div className='search-page'>

            <h1 className='search-title'>Search results: {searchTitle()}</h1>

            <div className='search-page-body'>

                <SearchPanel key={queryBody} query={queryBody} />

                {searchResult.length != 0 ?

                    <div className='lg:w-3/4 w-10/12 mb-5'>
                        <h3>
                            Showed {firstProductIndex()} - {lastProductIndex()} Of {searchData.total} results
                        </h3>

                        <div className='search-product-result'>
                            {searchResult.map(product => <SearchResultCard key={product.id} product={productDtoToProduct(product)} />)}
                        </div>

                        <Pagination
                            className='pagination'
                            color="primary"
                            variant="outlined"
                            count={Math.ceil(searchData.total / searchData.pageSize)}
                            page={Number(queryBody.pageIndex) + 1}
                            siblingCount={2}
                            onChange={navigateToPage}
                            showFirstButton
                            showLastButton
                            size='large'
                        />

                    </div>

                    :
                    <h1 className='lg:w-3/4 text-center mt-0 mb-5'>Sorry, nothing's here!</h1>
                }

            </div>


        </div>
    );
}