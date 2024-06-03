import { useApi } from "../../contexts/ApiContext";
import { Product } from "./ProductPage";
import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../../api/ApiManager";
import HttpClient from "../../api/signalR/HttpClient";
import { useEffect, useRef, useState } from "react";
import { BidDto } from "../../api/openapi-generated";
import { Button, Input, InputAdornment } from "@mui/material";
import NotificationSnackbar, { NotificationSnackbarProps } from "../NotificatonSnackbar";
import { useNavigate } from "react-router-dom";

import './BidPlacer.css';

export default function BidPlacer({ product }: { product: Product }) {

    const mounted = useRef(false);

    const navigate = useNavigate();

    const { api } = useApi();

    const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

    const [notification, setNotification] = useState<NotificationSnackbarProps | null>(null);

    const [price, setPrice] = useState<number>(0);

    const [bidHistory, setBidHistory] = useState<BidDto[]>([]);

    const [amount, setAmount] = useState(price);

    const handleAmountChange = (e) => {
        setAmount(e.target.value);
    };

    const putBid = () => {
        connection?.invoke("PlaceBid", { productId: product.id, amount: amount });
    };

    useEffect(() => {

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
            setBidHistory(bidHistory => [...bidHistory, bidDto]);

            setPrice(bidDto.amount);
        });

        connection.on("ReceiveError", error => {
            setNotification({ message: error, severity: "error" });
        });

        mounted.current = true;

        async function start() {
            try {
                await connection.start().then(_ => {
                    if (connection.connectionId) {

                        if (api.userIdentity) {
                            connection.invoke("AddToProductGroup", product.id.toString());
                        }

                        connection.invoke("GetLatestPrice", product.id);
                    }
                });
                setNotification({ message: "Connected to server", severity: "info" });
            } catch (err) {
                setNotification({ message: "Cannot connect to the server", severity: "error" });
            }
        };

        connection.onreconnecting(() => setNotification({ message: "Reconnecting...", severity: "warning" }));

        connection.onreconnected(() => setNotification({ message: "Reconnected!", severity: "success" }));

        start();

        setConnection(connection);

        return () => { connection.stop() };
    }, []);

    return (
        <div className="bid-placer-body">
            {api.userIdentity && <NotificationSnackbar message={notification?.message} severity={notification?.severity} />}
            <p>Current price: {price}</p>

            <div className="bid-control">
                <Button
                    className="bid-button bid-button-dec"
                    onClick={() => amount <= 0 ? 0 : setAmount(amount - 1)}
                    >-</Button>
                <Input
                    className="bid-input"
                    placeholder="Bid amount..."
                    value={amount}
                    onChange={handleAmountChange}
                    disableUnderline
                    // startAdornment={<InputAdornment className="bg-slate-300" position="start">$</InputAdornment>}
                    type="number" />
                <Button
                    className="bid-button bid-button-inc"
                    onClick={() => setAmount(amount + 1)}
                    >+</Button>

                <Button
                    className="button-submit"
                    onClick={api.userIdentity ? () => putBid() : () => navigate("/login")}
                >Place Bid
                </Button>
            </div>
        </div>
    );
}