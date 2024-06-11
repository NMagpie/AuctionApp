import { useCountdown } from "./useCountDown";
import { Typography } from "@mui/material";
import { ProductStatus } from "../Products/ProductPanel";
import { useEffect } from "react";

import './CountDown.css';

type CountDownProps = {
    targetDate: Date,
    productStatus: ProductStatus,
    setProductStatus: () => void,
};

export default function CountDown({ targetDate, setProductStatus }: CountDownProps) {

    const countDown = useCountdown(targetDate);

    useEffect(() => {
        if (countDown.every(e => e === 0)) {
            setProductStatus();
        }
    }, [countDown]);

    const unitList = [
        'Days',
        'Hours',
        'Minutes',
        'Seconds',
    ];

    const UnitNameList = () => unitList.map(
        unit => <Typography key={`unit-${unit}`}>{unit}</Typography>
    );

    const UnitValueList = () => countDown.map(
        (unit, index) => <Typography className="text-2xl lg:text-3xl" key={`value-${index}`}>{unit & unit}</Typography>
    );

    return (
        <div className="countdown-body">
            {<UnitValueList />}
            {<UnitNameList />}
        </div>
    );
}