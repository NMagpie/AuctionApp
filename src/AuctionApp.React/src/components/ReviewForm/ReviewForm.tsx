import { Button, Rating, Typography } from '@mui/material';
import { SetStateAction, useState } from 'react';
import { useApi } from '../../contexts/ApiContext';
import { ProductReviewDto } from '../../api/openapi-generated';

import './ReviewForm.css';

type ReviewFormProps = {
    productId: number,
    setReviews: React.Dispatch<SetStateAction<ProductReviewDto[]>>,
};

export default function ReviewForm({ productId, setReviews }: ReviewFormProps) {

    const { api } = useApi();

    const onSubmit = async () => {
        if (rating <= 0 || rating > 5) {
            setError("Set correct rating");
            return;
        }

        if (reviewText && reviewText.length > 2048) {
            setError("Review text cannot be longer that 2048 characters");
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
                setError(e.message)
            );
    };

    const [rating, setRating] = useState(0);

    const [reviewText, setReviewText] = useState<string | undefined>(undefined);

    const [error, setError] = useState("");

    return (
        <div className='review-form-body'>
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

            <Button className='submit-review' onClick={onSubmit}>Submit</Button>

            {error && <span className='text-red-500'>{error}</span>}
        </div>
    );
}