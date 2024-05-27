import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { LotDto, UserDto } from "../../api";
import { useApi } from "../../contexts/ApiContext";
import { Avatar, Typography } from "@mui/material";

import './AuctionPage.css';

type Auction = {
    id: number;
    title: string;
    creator: UserDto | null;
    startTime: Date | null;
    endTime: Date | null;
    lots: Array<LotDto>;
};

export default function AuctionPage() {

    const { id } = useParams();

    const [auction, setAuction] = useState<Auction | null>(null);

    const api = useApi().api;

    const getAuction = async () => {
        let { data } = await api.auctions.auctionsIdGet({ id: parseInt(id ?? "") });

        const auction = {
            id: data.id ?? 0,
            title: data.title ?? '',
            creator: data.creator ?? null,
            startTime: data.startTime ? new Date(data.startTime) : null,
            endTime: data.endTime ? new Date(data.endTime) : null,
            lots: data.lots ?? []
        };

        setAuction(auction);
    };

    console.log();

    useEffect(() => {
        getAuction();
    }, []);

    return (
        <div className="auction-info">
            <Typography className="font-bold" variant="h2">{auction?.title}</Typography>

            <div className="flex flex-row items-center">
                <Typography variant="h6" className="mr-5">Creator:</Typography>

                <Link className="flex flex-row items-center" to={`/users/${auction?.creator?.id}`}>
                    <Avatar className="mr-2" alt={auction?.creator?.userName?.charAt(0)} src="./src" />
                    <Typography variant="h6">{auction?.creator?.userName}</Typography>
                </Link>

            </div>

            <Typography variant="h6">Start Time: {auction?.startTime?.toLocaleString()}</Typography>
            <Typography variant="h6">End Time: {auction?.endTime?.toLocaleString()}</Typography>
        </div>
    )
}