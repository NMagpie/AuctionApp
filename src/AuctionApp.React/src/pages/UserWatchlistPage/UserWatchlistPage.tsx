import React, { useEffect, useState } from 'react'

import './UserWatchlistPage.css';
import { useLoaderData, useNavigate } from 'react-router-dom';
import { IconButton, Pagination, Tooltip } from '@mui/material';
import { ProductDto } from '../../api/openapi-generated';
import { productDtoToProduct } from '../../common';
import SearchResultCard from '../../components/Search/SearchResultCard';
import DeleteIcon from '@mui/icons-material/Delete';
import { useApi } from '../../contexts/ApiContext';
import { useSnackbar } from 'notistack';

function UserWatchlistPage() {

    const { userWatchlistData } = useLoaderData();

    const api = useApi();

    const navigate = useNavigate();

    const { enqueueSnackbar } = useSnackbar();

    const [userWatchlist, setUserWatchlist] = useState(userWatchlistData.items);

    const navigateToPage = (_, value: number) => {
        navigate(`/me/watchlist?pageIndex=${value - 1}`, { replace: true });

        window.scrollTo(0, 0);
    };

    useEffect(() => {
        setUserWatchlist(userWatchlistData.items);
    }, [userWatchlistData]);

    const deleteItem = async (productId: number) => {
        try {
            await api.userWatchlists.deleteUserWatchlistByProductId({ productId: productId });

            const updatedWatchlist = userWatchlist.filter((p: ProductDto) => p.id !== productId);

            setUserWatchlist(updatedWatchlist);

            enqueueSnackbar("Product has been deleted", {
                variant: "info"
            });
        } catch (e) {
            enqueueSnackbar(e, {
                variant: "error"
            });
        }
    };

    return (
        <div className="user-watchlist-body">
            <h1>My Watchlist</h1>

            {userWatchlistData.total !== 0 ?
                <>

                    <div className="user-watchlist-results">
                        {userWatchlist.map((product: ProductDto) => {

                            return (
                                <div className="relative" key={`user-product-${product.id}`}>
                                    <Tooltip
                                        className="bg-slate-700 text-white"
                                        onClick={() => { deleteItem(product.id); }}
                                        title="Delete"
                                    >
                                        <IconButton
                                            className={"product-delete-button right-1"} >
                                            <DeleteIcon />
                                        </IconButton>
                                    </Tooltip>

                                    <SearchResultCard
                                        product={productDtoToProduct(product)}
                                    />
                                </div>
                            );

                        })
                        }
                    </div>

                    <Pagination
                        className='pagination'
                        color="primary"
                        variant="outlined"
                        count={Math.ceil(userWatchlistData.total / userWatchlistData.pageSize)}
                        page={Number(userWatchlistData.pageIndex) + 1}
                        siblingCount={2}
                        onChange={navigateToPage}
                        showFirstButton
                        showLastButton
                        size='large'
                    />
                </>
                :
                <h2>Well, Watchlist is empty!</h2>
            }

        </div>
    )
}

export default UserWatchlistPage;