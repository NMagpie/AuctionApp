import { useContext, useEffect, useState } from "react";
import { AuctionDto } from "../../api";
import { ApiContext } from "../../contexts/ApiContext";

const HomePage = () => {

    const apiProvider = useContext(ApiContext);

    return (
        <div>
            <h1>Hello{apiProvider.user?.userName ?  `, ${apiProvider.user?.userName}` : ""}!</h1>
            <AuctionInfo />
        </div>
    );
};

export default HomePage;

function AuctionInfo() {

    const [auction, setAuction] = useState<AuctionDto | null>(null);

    const apiProvider = useContext(ApiContext);

    const getAuction = async () => {
        let result = await apiProvider.auctions.auctionsIdGet({ id: 1 });

        setAuction(result.data)
    };

    useEffect(() => {
        getAuction();
    }, []);

    return (
        <p>{JSON.stringify(auction)}</p>
    )
}