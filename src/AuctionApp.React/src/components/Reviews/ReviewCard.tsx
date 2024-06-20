import { Avatar, Rating, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { ProductReviewDto } from "../../api/openapi-generated";

import './ReviewCard.css';

export default function ReviewCard({ review }: { review: ProductReviewDto }) {

    return (
        <div className="review-body">

            <Link
                className="flex flex-row mb-2 w-fit"
                onClick={() => { window.scrollTo(0, 0); }}
                to={`/users/${review.user?.id}`}>
                <Avatar
                    className="mr-2"
                    alt={review?.user?.userName?.charAt(0)}
                    src="./src" />
                <Typography variant="h6">{review?.user?.userName}</Typography>
            </Link>


            <Rating
                className="mb-3"
                precision={0.5}
                size='large'
                value={review.rating}
                readOnly
            />

            <Typography>
                {review.reviewText}
            </Typography>

        </div>
    );
}