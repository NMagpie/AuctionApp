import React, { useState, useEffect } from "react";
import { Alert, Button, Typography } from "@mui/material";

import { useApi } from "../contexts/ApiContext";
import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../api/ApiManager";
import HttpClient from "../api/signalR/HttpClient";

type PutBidRequest = {
    LotId: number,
    Amount: number,
};

const BidsTestPage: React.FC = () => {

    const [error, setError] = useState("");

    const { api } = useApi();

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseUrl}/BidsHub`, {
            httpClient: new HttpClient(api),
        })
        .configureLogging(signalR.LogLevel.Information)
        .withAutomaticReconnect()
        .build();

    connection.start().then(_ => {
        if (connection.connectionId) {
            connection.invoke("AddToLotGroup", "1");
        }
    });

    const SignalRClient: React.FC = () => {
        const [clientMessage, setClientMessage] = useState<string | null>(null);

        useEffect(() => {
            connection.on("BidNotify", message => {
                setClientMessage(message);
            });
            connection.on("ReceiveError", error => {
                setError(error);
            });
        });

        const putBid = (request: PutBidRequest) => {
            connection.invoke("PutBid", request);
        }

        return (
            <>
                <p>{clientMessage}</p>
                <Button onClick={() => {
                    putBid({ LotId: 1, Amount: 4.5 });
                }}
                >Put Bid</Button>
                {error && <Alert severity="error">{error.toString()}</Alert>}
            </>
        );
    };


    return <SignalRClient />;
};

export default BidsTestPage;