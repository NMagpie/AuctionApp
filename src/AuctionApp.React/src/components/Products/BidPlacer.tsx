import { useApi } from "../../contexts/ApiContext";
import { Product } from "../../pages/ProductPage/ProductPage";
import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../../api/ApiManager";
import { useEffect, useState } from "react";
import { BidDto } from "../../api/openapi-generated";
import { AlertColor, Button, InputAdornment, TextField } from "@mui/material";
import NotificationSnackbar, { NotificationSnackbarProps } from "../NotificatonSnackbar";
import { useNavigate } from "react-router-dom";
import GavelIcon from '@mui/icons-material/Gavel';

import './BidPlacer.css';

export default function BidPlacer({ product }: { product: Product }) {

    const navigate = useNavigate();

    const { api } = useApi();

    const [connection, setConnection] = useState<signalR.HubConnection | null>();

    const setOpenNotification = (state: boolean) => {
        setNotification(prevState => {
            return {
                ...prevState,
                open: state,
            };
        });
    };

    const setNotificationMessage = (message: string, severity: AlertColor) => {
        setNotification(prevState => {
            return {
                ...prevState,
                message: message,
                severity: severity,
                open: true,
            };
        });
    };

    const [notification, setNotification] = useState<NotificationSnackbarProps>({ message: "", open: false, setOpen: setOpenNotification });

    const [price, setPrice] = useState<number>(0);

    const [bidHistory, setBidHistory] = useState<BidDto[]>([]);

    const [amount, setAmount] = useState<number>(price);

    const handleAmountChange = (e) => {
        setAmount(e.target.value);
    };

    const placeBid = () => {
        connection
            ?.invoke("PlaceBid", { productId: product.id, amount: amount })
            .catch(error => {
                setNotificationMessage(error.message, "error");
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
            setNotificationMessage(error, "error");
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
                setNotificationMessage("Connected", "info");
            } catch (err) {
                setNotificationMessage("Cannot connect to the server", "error");
            }
        }

        connection.onreconnecting(() => setNotificationMessage("Reconnecting...", "warning"));

        connection.onreconnected(() => setNotificationMessage("Reconnected!", "success"));

        start();

        setConnection(connection);

        return () => { connection.stop() };
    }, []);

    return (
        <div className="bid-placer-body">
            <NotificationSnackbar message={notification?.message} severity={notification?.severity} open={notification?.open} setOpen={setOpenNotification} />
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
                    onClick={api.userIdentity ? () => placeBid() : () => navigate("/login")}
                >
                    <GavelIcon className="mr-2"/>
                    Bid
                </Button>
            </form>
        </div >
    );
}