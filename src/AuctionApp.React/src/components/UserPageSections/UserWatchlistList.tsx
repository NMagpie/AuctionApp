import { Tooltip, IconButton, Typography } from '@mui/material';
import { useState } from 'react'
import { ProductDto } from '../../api/openapi-generated';
import { productDtoToProduct } from '../../common';
import SearchResultCard from '../Search/SearchResultCard';
import { useSnackbar } from 'notistack';
import { useApi } from '../../contexts/ApiContext';
import DeleteIcon from '@mui/icons-material/Delete';

import './UserProductList.css';

function UserWatchlistList({ products }) {

    const api = useApi();

    const { enqueueSnackbar } = useSnackbar();

    const [userWatchlist, setUserWatchlist] = useState(products);

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
        userWatchlist.length !== 0 ?
            <>
                <Typography className="font-bold" variant="h5">Watchlist:</Typography>

                <div className="user-product-list">
                    {userWatchlist.map((product: ProductDto) => {

                        return (
                            <div className="relative" key={`user-product-${product.id}`}>
                                <Tooltip
                                    className="bg-slate-700 text-white"
                                    onClick={() => { deleteItem(product.id); }}
                                    title="Delete"
                                >
                                    <IconButton
                                        className={"product-manage-button right-1"} >
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

            </>
            :
            <h2>Well, The Watchlist is empty!</h2>
    )
}

export default UserWatchlistList