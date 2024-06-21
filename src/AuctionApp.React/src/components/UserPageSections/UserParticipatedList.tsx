import { Tooltip, IconButton, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { ProductDto } from '../../api/openapi-generated';
import { productDtoToProduct } from '../../common';
import SearchResultCard from '../Search/SearchResultCard';
import EmojiEventsIcon from '@mui/icons-material/EmojiEvents';

import './UserProductList.css';
import { useApi } from '../../contexts/ApiContext';

function UserParticipatedList({ products, userId }) {

    const api = useApi()

    const [productsState, setProductsState] = useState(products);

    useEffect(() => {
        setProductsState(products);
    }, [products]);

    const userWon = (product: ProductDto) => Boolean(product.bids?.filter(
        b => b.isWon &&
            b.userId == userId &&
            new Date(product.endTime) < new Date()
    ).length);

    const isCurrentUser = userId === api.user?.id;

    return (
        productsState.length ?
            <>
                <Typography className="font-bold" variant="h5">Participated in:</Typography>

                <div className="user-product-list">

                    {productsState.map((product: ProductDto) => {

                        return (
                            <div className="relative" key={`user-product-${product.id}`}>

                                {userWon(product) && <Tooltip
                                    className="text-white bg-emerald-700"
                                    title={`${isCurrentUser ? "You" : "The user"} won the auction`}
                                >
                                    <IconButton
                                        className="product-manage-button right-1" >
                                        <EmojiEventsIcon />
                                    </IconButton>
                                </Tooltip>}

                                <SearchResultCard
                                    product={productDtoToProduct(product)}
                                />

                            </div>
                        );
                    }
                    )}
                </div>
            </>
            :
            <h2>Empty!</h2>
    );
}

export default UserParticipatedList;