import { Button, Rating, Typography } from '@mui/material';
import { SetStateAction, useState } from 'react';
import { useApi } from '../../contexts/ApiContext';
import { ProductReviewDto } from '../../api/openapi-generated';
import { useSnackbar } from 'notistack';

import './ReviewForm.css';

type ReviewFormProps = {
    productId: number,
    review?: ProductReviewDto,
    setReviews: React.Dispatch<SetStateAction<ProductReviewDto[]>>,
    setEditMode: React.Dispatch<SetStateAction<boolean>>,
    setReviewState: React.Dispatch<React.SetStateAction<ProductReviewDto>>,
};

export default function ReviewForm({ productId, review, setReviews, setEditMode, setReviewState }: ReviewFormProps) {

    const api = useApi();

    const { enqueueSnackbar } = useSnackbar();

    const [rating, setRating] = useState(review?.rating ?? 0);

    const [reviewText, setReviewText] = useState<string | null>(review?.reviewText ?? "");

    const onSubmit = async (e) => {
        e.preventDefault();

        if (rating <= 0 || rating > 5) {
            enqueueSnackbar("Set correct rating", {
                variant: "error"
            });
            return;
        }

        if (reviewText && reviewText.length > 2048) {
            enqueueSnackbar("Review text cannot be longer that 2048 characters", {
                variant: "error"
            });
            return;
        }

        const invokeRequest = review ?
            api.productReviews.updateProductReview.bind(api.productReviews) :
            api.productReviews.createProductReview.bind(api.productReviews);

        const requestType = review ? "updateProductReviewRequest" : "createProductReviewRequest";

        const requestBody = {
            ...review && { id: review.id },
            [requestType]: {
                productId: productId,
                rating: rating,
                reviewText: reviewText,
            }
        };

        invokeRequest(requestBody)
            .then(({ data }) => {

                if (!review) {
                    setRating(0);
                    setReviewText("");

                    setReviews((prevReviews) => [
                        data,
                        ...prevReviews
                    ]);
                } else {
                    setReviewState((prevState: ProductReviewDto) => {
                        return {
                            ...prevState,
                            rating: rating,
                            ...reviewText && { reviewText },
                        };
                    });

                    setEditMode(false);
                }

            })
            .catch(e => {
                const msg = e?.response?.status === 422 ? "Cannot put review: product sell is not finished" : e;

                enqueueSnackbar(msg, {
                    variant: "error"
                });
            }
            );
    };

    return (
        <form className={`${review ? "bg-white p-5" : "bg-slate-100"} review-form-body`}>
            <h3 className='mt-0'>
                {review ? "Edit review" : "How was your bidding? Please, leave a review:"}
            </h3>

            <Rating
                precision={0.5}
                size='large'
                value={rating}
                onChange={(event, newValue) => {
                    setRating(newValue);
                }}
            />

            <Typography className='mt-4 font-semibold'>Your review:</Typography>

            <textarea
                rows={10}
                value={reviewText}
                onChange={(event) => {
                    setReviewText(event.target.value);
                }}
            />

            <Button
                type='submit'
                className='submit-review'
                onClick={onSubmit}
            >Submit
            </Button>
        </form>
    );
}