import { useEffect, useState } from 'react';

const useCountdown = (targetDate: Date) => {

    const countDownDate = targetDate?.getTime();

    const [initialCountDown] = useState(countDownDate - new Date().getTime());

    const [countDown, setCountDown] = useState(
        initialCountDown
    );


    useEffect(() => {

        if (countDownDate) {
            setCountDown(countDownDate - new Date().getTime());
        }

        setInterval(() => {
                setCountDown(countDownDate - new Date().getTime());
            }, 1000);

        return () => clearInterval(countDown);
    }, [countDownDate]);

    return getReturnValues(countDown);
};

const getReturnValues = (countDown: number) => {
    const days = Math.floor(countDown / (1000 * 60 * 60 * 24));
    const hours = Math.floor(
        (countDown % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
    );
    const minutes = Math.floor((countDown % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((countDown % (1000 * 60)) / 1000);

    return [days, hours, minutes, seconds];
};

export { useCountdown };