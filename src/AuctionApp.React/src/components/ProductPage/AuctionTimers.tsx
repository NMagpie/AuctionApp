import { Typography } from "@mui/material";
import Countdown from "./Countdown";
import { Product } from "./ProductPage";

const dateFormat = { year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' };

export default function AuctionTimers({ product }: { product: Product }) {

    const isSellingEnded = () => product?.endTime <= new Date();

    const isSellingStarted = () => product?.startTime <= new Date();

    return (
        <>
            {isSellingEnded() ?
                <div className="flex flex-col items-center">
                    <Typography variant="h6">{`End Time: ${product?.endTime?.toLocaleTimeString([], dateFormat)}`}</Typography>
                    <Typography variant="h5">
                        Auction has ended
                    </Typography>
                </div>
                :
                <>
                    <div className="flex flex-col items-center">
                        <Typography variant="h6">
                            {isSellingStarted() ?
                                `End Time: ${product?.endTime?.toLocaleTimeString([], dateFormat)}`
                                :
                                `Start Time: ${product?.startTime?.toLocaleTimeString([], dateFormat)}`
                            }
                        </Typography>

                        <Typography className="font-bold mt-3" variant="h6">{isSellingStarted() ? "Finishing in:" : "Starting in:"}</Typography>
                    </div>

                    <Countdown targetDate={isSellingStarted() ? product?.endTime : product?.startTime} />
                </>
            }
        </>
    );
};