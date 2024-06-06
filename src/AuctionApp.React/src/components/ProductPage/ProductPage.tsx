import { useState } from "react";
import { Link, useLoaderData } from "react-router-dom";
import { useApi } from "../../contexts/ApiContext";
import { Avatar, Button, Typography } from "@mui/material";
import { BidDto, UserDto, UserWatchlistDto } from "../../api/openapi-generated";
import ProductPanel from "./ProductPanel";
import BookmarkIcon from '@mui/icons-material/Bookmark';
import BookmarkRemoveIcon from '@mui/icons-material/BookmarkRemove';

import './ProductPage.css';

export type Product = {
    id: number;
    title: string;
    description: string;
    creator: UserDto | null;
    startTime: Date | null;
    endTime: Date | null;
    bids: BidDto[];
};

export default function ProductPage() {

    const { productData, watchlistData } = useLoaderData();

    const [product] = useState<Product>(productData);

    const [watchlist, setWatchlist] = useState<UserWatchlistDto | null>(watchlistData);

    const { api } = useApi();

    const addWatchlist = async () => {
        const { data } = await api.userWatchlsits.createUserWatchlist({ createUserWatchlistRequest: { productId: product?.id } });

        setWatchlist(data);
    };

    const removeWatchlist = async () => {
        await api.userWatchlsits.deleteUserWatchlist({ id: watchlist?.id });

        setWatchlist(null);
    }

    return (
        <div className="product-card">

            <div className="product-info">
                <img src="https://bidpro.webdevia.com/wp-content/uploads/2018/05/alexander-andrews-BX4Q0gojWAs-unsplash.jpg" alt={`product-${product?.id}-img`}></img>

                <Typography
                    className="product-title hidden lg:inline"
                    variant="h2">{product?.title}</Typography>
                <Typography
                    className="product-title lg:hidden"
                    variant="h3">{product?.title}</Typography>

                <Typography
                    className="font-bold">{product?.description}</Typography>
            </div>

            <div className="product-brief">
                <Typography
                    className="product-brief-title hidden lg:block"
                    variant="h3">{product?.title?.substring(0, 50)}</Typography>
                <Typography
                    className="product-brief-title lg:hidden"
                    variant="h4">{product?.title?.substring(0, 50)}</Typography>

                <div className="flex flex-row items-baseline">
                    <Typography
                        variant="h6"
                        className="mr-3"
                    >Creator:
                    </Typography>

                    <Link
                        className="flex flex-row"
                        to={`/users/${product?.creator?.id}`}>
                        <Avatar
                            className="mr-2"
                            alt={product?.creator?.userName?.charAt(0)}
                            src="./src" />
                        <Typography variant="h6">{product?.creator?.userName}</Typography>
                    </Link>
                </div>

                <div className="flex flex-col items-center">
                    {api.userIdentity &&
                        <Button
                            className="watchlist-button"
                            variant="contained"
                            startIcon={watchlist ? <BookmarkRemoveIcon /> : <BookmarkIcon />}
                            onClick={watchlist ? removeWatchlist : addWatchlist}>
                            {watchlist ? "Out of Watchlist" : "To Watchlist"}
                        </Button>
                    }
                </div>

                <div className="product-brief-divider" />

                {product && <ProductPanel product={product} />}

            </div>

        </div>
    )
}