import { useState } from "react";
import { Link, useLoaderData } from "react-router-dom";
import { useApi } from "../../contexts/ApiContext";
import { Avatar, Button, Typography } from "@mui/material";
import { ProductReviewDto } from "../../api/openapi-generated";
import ProductPanel from "../../components/Products/ProductPanel";
import BookmarkIcon from '@mui/icons-material/Bookmark';
import BookmarkRemoveIcon from '@mui/icons-material/BookmarkRemove';
import ReviewForm from "../../components/Reviews/ReviewForm";
import InfiniteScroll from "react-infinite-scroll-component";
import { Product, getProductImage, hasMore } from "../../common";
import ReviewItem from "../../components/Reviews/ReviewItem";

import './ProductPage.css';
import { Image } from "@mui/icons-material";

export default function ProductPage() {

    const { productData, watshlistExists, reviewsData, canUserLeaveReview } = useLoaderData();

    const [product] = useState<Product>(productData);

    const [watchlist, setWatchlist] = useState<boolean>(watshlistExists);

    const [reviews, setReviews] = useState<Array<ProductReviewDto>>(reviewsData.items);

    const [reviewsIndex, setReviewsIndex] = useState(0);

    const [hasMoreReviews, setHasMoreReviews] = useState(hasMore(0, reviewsData.pageSize, reviewsData.total));

    const api = useApi();

    const addWatchlist = async () => {
        await api.userWatchlists.createUserWatchlist({ createUserWatchlistRequest: { productId: product?.id } });

        setWatchlist(true);
    };

    const removeWatchlist = async () => {
        await api.userWatchlists.deleteUserWatchlistByProductId({ productId: product?.id });

        setWatchlist(false);
    }

    const fetchReviews = async () => {

        const nextIndex = reviewsIndex + 1;

        const { data } = await api.productReviews.getPagedReviews({ productId: product?.id, pageIndex: nextIndex });

        setReviews([
            ...reviews,
            ...data.items
        ]);

        setReviewsIndex(nextIndex);

        setHasMoreReviews(hasMore(nextIndex, reviewsData.pageSize, data.total));
    };

    return (
        <div className="product-card">

            <div className="product-brief">
                <Typography
                    className="product-brief-title hidden lg:block"
                    variant="h3">
                    {product?.title?.substring(0, 50)}
                </Typography>

                <Typography
                    className="product-brief-title lg:hidden"
                    variant="h4">
                    {product?.title?.substring(0, 50)}
                </Typography>

                <div className="creator-info">
                    <Typography
                        variant="h6"
                        className="mr-3"
                    >Creator:
                    </Typography>

                    <Link
                        className="flex flex-row"
                        onClick={() => { window.scrollTo(0, 0); }}
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

                <div className="text-flex-divider my-5" />

                {product && <ProductPanel product={product} />}

            </div>

            <div className="product-body">

                <img
                    src={getProductImage(product.category)}
                    alt={`product-${product?.id}-img`}
                />

                <Typography
                    className="product-title hidden lg:inline"
                    variant="h2">
                    {product?.title}
                </Typography>

                <Typography
                    className="product-title lg:hidden"
                    variant="h3">
                    {product?.title}
                </Typography>

                <Typography
                    className="text-xl font-medium mb-5">
                    {product?.description}
                </Typography>

            </div>

            <div className="review-section">

                {canUserLeaveReview && <ReviewForm productId={product.id} setReviews={setReviews} />}

                <div className="reviews-list">
                    {reviews.length ?
                        <InfiniteScroll
                            dataLength={reviews.length}
                            next={fetchReviews}
                            hasMore={hasMoreReviews}
                            loader={<h4>Loading...</h4>}

                        >
                            {reviews.map(review =>
                                <ReviewItem
                                    key={review.id}
                                    review={review}
                                    isEditable={review.user?.id === api.user?.id}
                                    setReviews={setReviews}
                                />)}
                        </InfiniteScroll>
                        :
                        <h2>No reviews</h2>
                    }
                </div>
            </div>

        </div>
    )
}