import { AlertColor, IconButton, Snackbar, Alert } from "@mui/material";
import CloseIcon from '@mui/icons-material/Close';
import { useState, useEffect } from "react";

export type NotificationSnackbarProps = {
    message: string,
    duration?: number,
    severity?: AlertColor,
};

export default function NotificationSnackbar({ message, duration = 5000, severity = "info" }: NotificationSnackbarProps) {
    const [open, setOpen] = useState(false);

    useEffect(() => {
        setOpen(Boolean(message));
    }, [message]);

    const handleClose = (event: React.SyntheticEvent | Event, reason?: string) => {
        if (reason === 'clickaway') {
            return;
        }

        setOpen(false);
    };

    const action = (
        <IconButton
            size="small"
            aria-label="close"
            color="inherit"
            onClick={handleClose}
        >
            <CloseIcon fontSize="small" />
        </IconButton>
    );

    return (
        <Snackbar
            open={open}
            autoHideDuration={duration}
            onClose={handleClose}
            action={action}
        >
            <Alert variant="filled" severity={severity}>
                {message}
            </Alert>
        </Snackbar>
    );
}