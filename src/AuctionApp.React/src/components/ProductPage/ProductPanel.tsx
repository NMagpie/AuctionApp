import { Typography } from "@mui/material";
import Countdown from "./SellingCountdown";
import { Product } from "./ProductPage";
import BidPlacer from "./BidPlacer";
import { useState } from "react";

const dateFormat = { year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' };

export type ProductStatus = "pending" | "started" | "finished";

const getProductStatus = (product: Product): ProductStatus => {

    if (!product) {
        return null;
    }

    const currentDate = new Date();

    const isSellingStarted = product?.startTime <= currentDate;

    const isSellingEnded = product?.endTime <= currentDate;

    switch (true) {
        case (!isSellingStarted && !isSellingEnded):
            return "pending"
        case (isSellingStarted && !isSellingEnded):
            return "started"
        case (isSellingStarted && isSellingEnded):
            return "finished";
    }
};

export default function ProductPanel({ product }: { product: Product }) {

    const [productStatus, setProductStatus] = useState<ProductStatus>(getProductStatus(product));

    return (
        <>
            {productStatus === "finished" ?
                <div className="flex flex-col items-center">
                    <Typography
                        variant="h6">
                        {`End Time: ${product?.endTime?.toLocaleTimeString([], dateFormat)}`}
                    </Typography>
                    <Typography variant="h5">
                        Auction has ended
                    </Typography>
                </div>
                :
                <>
                    <div className="flex flex-col items-center">
                        <Typography variant="h6">
                            {productStatus === "started" ?
                                `End Time: ${product?.endTime?.toLocaleTimeString([], dateFormat)}`
                                :
                                `Start Time: ${product?.startTime?.toLocaleTimeString([], dateFormat)}`
                            }
                        </Typography>

                        <Typography
                            className="font-bold mt-3"
                            variant="h6">
                            {productStatus === "started" ? "Finishing in:" : "Starting in:"}
                        </Typography>
                    </div>

                    {product &&
                        <Countdown
                            key={productStatus === "started" ? "finish-timer" : "start-timer"}
                            setProductStatus={productStatus === "started" ? () => { setProductStatus("finished") } : () => { setProductStatus("started") }
                            }
                            targetDate={productStatus === "started" ? product?.endTime : product?.startTime} />}

                    {productStatus === "started" && <BidPlacer product={product} />}
                </>
            }
        </>
    );
};