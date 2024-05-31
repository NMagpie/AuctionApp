import { useApi } from "../../contexts/ApiContext";
import { Product } from "./ProductPage";
import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../../api/ApiManager";
import HttpClient from "../../api/signalR/HttpClient";
import { useEffect, useState } from "react";
import { BidDto } from "../../api/openapi-generated";

import './BidPlacer.css';
import NotificationSnackbar, { NotificationSnackbarProps } from "../NotificatonSnackbar";

type CreateBidRequest = {
    LotId: number,
    Amount: number,
};

export default function BidPlacer({ product }: { product: Product }) {

    const [notification, setNotification] = useState<NotificationSnackbarProps | null>(null);

    const [price, setPrice] = useState<number>(0);

    const [bidHistory, setBidHistory] = useState<BidDto[]>([]);

    const [latestBid, setLatestBid] = useState<BidDto | null>(null);

    const { api } = useApi();

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseUrl}/BidsHub`, {
            httpClient: new HttpClient(api),
        })
        .configureLogging(signalR.LogLevel.Information)
        .withAutomaticReconnect()
        .build();

    connection.on("GetLatestPrice", (price: number) => {
        setPrice(price);
    });

    connection.on("BidNotify", (bidDto: BidDto) => {
        //setBidHistory(bidHistory => [...bidHistory, bidDto]);

        setLatestBid(bidDto);
        setPrice(bidDto.amount);
    });

    connection.on("ReceiveError", error => {
        setNotification({ message: error, severity: "error" });
    });

    const putBid = (request: CreateBidRequest) => {
        connection.invoke("PutBid", request);
    };

    useEffect(() => {
        async function start() {
            try {
                await connection.start().then(_ => {
                    if (connection.connectionId) {
                        connection.invoke("AddToProductGroup", product.id);
                        connection.invoke("GetLatestPrice", product.id);
                    }
                });
                setNotification({ message: "Connected to server", severity: "info" });
            } catch (err) {
                //setNotification({ message: "Cannot connect to the server", severity: "error" });
                if (connection.state == signalR.HubConnectionState.Disconnected)
                    setTimeout(start, 5000);
            }
        };

        connection.onreconnecting(() => setNotification({ message: "Reconnecting...", severity: "warning" }));

        connection.onreconnected(() => setNotification({ message: "Reconnected!", severity: "success" }));

        start();
    }, []);

    return (
        <div className="bid-placer-body">
            <NotificationSnackbar message={notification?.message} severity={notification?.severity} />
            {price}
        </div>
    );
}