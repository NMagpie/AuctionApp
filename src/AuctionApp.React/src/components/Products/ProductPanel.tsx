import { Typography } from "@mui/material";
import BidPlacer from "./BidPlacer";
import { useState } from "react";
import CountDown from "../CountDown/CountDown";
import { Product, ProductStatus, dateFormat, getProductStatus } from "../../common";

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
                        Selling has ended
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
                        <CountDown
                            key={productStatus === "started" ? "finish-timer" : "start-timer"}
                            setProductStatus={
                                productStatus === "started" ? () => { setProductStatus("finished") } : () => { setProductStatus("started") }
                            }
                            targetDate={productStatus === "started" ? product?.endTime : product?.startTime} />}

                    {productStatus === "started" && <BidPlacer product={product} />}
                </>
            }
        </>
    );
}