import { useApi } from "../../contexts/ApiContext";
import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../../api/ApiManager";
import { useEffect, useState } from "react";
import { BidDto } from "../../api/openapi-generated";
import { Button, InputAdornment, TextField } from "@mui/material";
import { useNavigate } from "react-router-dom";
import GavelIcon from '@mui/icons-material/Gavel';
import { useSnackbar } from "notistack";
import { Product } from "../../common";

import './BidPlacer.css';

export default function BidPlacer({ product }: { product: Product }) {

    const navigate = useNavigate();

    const api = useApi();

    const [connection, setConnection] = useState<signalR.HubConnection | null>();

    const { enqueueSnackbar } = useSnackbar();

    const [price, setPrice] = useState<number>(0);

    const [bidHistory, setBidHistory] = useState<BidDto[]>([]);

    const [amount, setAmount] = useState<number>(price);

    const handleAmountChange = (e) => {
        setAmount(Number(e.target.value));
    };

    const placeBid = (e) => {
        e.preventDefault();
        connection
            ?.invoke("PlaceBid", { productId: product.id, amount: amount })
            .catch(error => {
                enqueueSnackbar(error.message, {
                    variant: "error"
                });
            });
    };

    useEffect(() => {

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${baseUrl}/BidsHub`, {
                accessTokenFactory: async () => await api.retrieveAccessToken(),
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
            enqueueSnackbar(error, {
                variant: "error"
            });
        });

        async function start() {
            try {
                await connection.start().then(() => {
                    if (connection.connectionId) {

                        if (api.userIdentity) {
                            connection.invoke("AddToProductGroup", product.id.toString());
                        }

                        connection.invoke("GetLatestPrice", product.id);
                    }
                });
                enqueueSnackbar("Connected", {
                    variant: "info"
                });
            } catch (err) {
                console.log(err);
            }
        }

        connection.onreconnecting(() => enqueueSnackbar("Reconnecting...", {
            variant: "warning"
        }));

        connection.onreconnected(() => enqueueSnackbar("Reconnected!", {
            variant: "success"
        }));

        start();

        setConnection(connection);

        return () => { connection.stop() };
    }, []);

    return (
        <div className="bid-placer-body">
            <p className="font-bold text-lg">Current price: {price}</p>

            <form className="bid-control">

                <div className="flex justify-center mx-3">
                    <Button
                        className="bid-button bid-button-dec"
                        onClick={() => amount <= 0 ? setAmount(0) : setAmount(amount - 1)}
                    >-
                    </Button>

                    <TextField
                        className="bid-input"
                        placeholder="Bid amount..."
                        value={amount}
                        onChange={handleAmountChange}
                        type="number"
                        InputProps={{
                            inputProps: { min: 0 },
                            endAdornment: (
                                <InputAdornment className="bg-slate-300" position="end">$</InputAdornment>
                            )
                        }}
                    />

                    <Button
                        className="bid-button bid-button-inc"
                        onClick={() => setAmount(Number(amount) + 1)}
                    >+
                    </Button>

                </div>

                <Button
                    type="submit"
                    className="button-submit"
                    onClick={api.userIdentity ? (e) => placeBid(e) : () => navigate("/login")}
                >
                    <GavelIcon className="mr-2" />
                    Bid
                </Button>
            </form>
        </div >
    );
}