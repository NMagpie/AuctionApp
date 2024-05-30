import { useCountdown } from "./useCountDown";
import { Typography } from "@mui/material";

import './Countdown.css';

export default function Countdown({ targetDate }: { targetDate: Date }) {

    const countDown = useCountdown(targetDate);

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