import { useEffect, useState } from "react";
import { AuctionDto } from "../../api";
import { User, useApi } from "../../contexts/ApiContext";

const HomePage = () => {

    const {api, didUserLoad} = useApi();

    const [user, setUser] = useState<User | null>(api.user);

    useEffect(() => {
        setUser(api.user);
    }, [didUserLoad]);

    return (
        <div>
            <h1>Hello{user?.userName && `, ${user?.userName}`}!</h1>
        </div>
    );
};

export default HomePage;