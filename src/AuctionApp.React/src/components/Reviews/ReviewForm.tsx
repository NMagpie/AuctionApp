import { Button, Rating, Typography } from '@mui/material';
import { SetStateAction, useState } from 'react';
import { useApi } from '../../contexts/ApiContext';
import { ProductReviewDto } from '../../api/openapi-generated';
import { useSnackbar } from 'notistack';

import './ReviewForm.css';

type ReviewFormProps = {
    productId: number,
    setReviews: React.Dispatch<SetStateAction<ProductReviewDto[]>>,
};

export default function ReviewForm({ productId, setReviews }: ReviewFormProps) {

    const api = useApi();

    const { enqueueSnackbar } = useSnackbar();

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

        api.productReviews
            .createProductReview({
                createProductReviewRequest:
                {
                    productId: productId,
                    rating: rating,
                    reviewText: reviewText,
                }
            })
            .then(({ data }) => {
                setRating(0);
                setReviewText("");

                setReviews((prevReviews) => [
                    data,
                    ...prevReviews
                ]);
            })
            .catch(e =>
                enqueueSnackbar(e.message, {
                    variant: "error"
                })
            );
    };

    const [rating, setRating] = useState(0);

    const [reviewText, setReviewText] = useState<string | undefined>(undefined);

    return (
        <form className='review-form-body'>
            <h3 className='mt-0'>
                Leave a review:
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