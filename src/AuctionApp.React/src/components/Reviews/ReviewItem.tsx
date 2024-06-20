import { Tooltip, IconButton } from "@mui/material";
import { useState } from "react";
import { ProductReviewDto } from "../../api/openapi-generated";
import ReviewCard from "./ReviewCard";
import ReviewForm from "./ReviewForm";
import DeleteIcon from '@mui/icons-material/Delete';
import CancelIcon from '@mui/icons-material/Cancel';
import EditIcon from '@mui/icons-material/Edit';

import './ReviewItem.css';
import { useApi } from "../../contexts/ApiContext";
import { useSnackbar } from "notistack";

type ReviewItemProps = {
    review: ProductReviewDto,
    isEditable: boolean,
    setReviews: React.Dispatch<React.SetStateAction<ProductReviewDto[]>>,
};

function ReviewItem({ review, isEditable, setReviews }: ReviewItemProps) {

    const api = useApi();

    const { enqueueSnackbar } = useSnackbar();

    const [editMode, setEditMode] = useState(false);

    const [reviewState, setReviewState] = useState(review);

    const deleteReview = async () => {
        try {
            api.productReviews.deleteProductReview({ id: review.id, });

            enqueueSnackbar("Review was deleted", {
                variant: "info"
            });
        } catch (e) {
            enqueueSnackbar(e, {
                variant: "error"
            });
        }

        setReviews((prevState: ProductReviewDto[]) => {
            const updatedState = prevState.filter(r => r.id !== review.id);

            return updatedState;
        });
    };

    return (
        <div className="review-item">
            {editMode ?
                <div className="my-5">

                    <Tooltip
                        className="bg-slate-700 text-white"
                        onClick={() => { setEditMode(false); }}
                        title="Close"
                    >
                        <IconButton
                            className="review-manage-button right-1" >
                            <CancelIcon />
                        </IconButton>

                    </Tooltip>

                    <ReviewForm
                        review={reviewState}
                        productId={review.productId}
                        setReviews={setReviews}
                        setEditMode={setEditMode}
                        setReviewState={setReviewState}
                    />
                </div>
                :
                <>
                    {isEditable &&
                        <>
                            <Tooltip
                                className="bg-slate-700 text-white"
                                onClick={deleteReview}
                                title="Delete"
                            >

                                <IconButton
                                    className="review-manage-button right-1" >
                                    <DeleteIcon />
                                </IconButton>

                            </Tooltip>

                            <Tooltip
                                className="bg-slate-700 text-white"
                                onClick={() => { setEditMode(true); }}
                                title="Edit"
                            >

                                <IconButton
                                    className="review-manage-button right-12" >
                                    <EditIcon />
                                </IconButton>

                            </Tooltip>
                        </>
                    }
                    <ReviewCard review={reviewState} />
                </>
            }
        </div>
    );
}

export default ReviewItem;