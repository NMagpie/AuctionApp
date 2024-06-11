import { AlertColor, Snackbar, Alert } from "@mui/material";

export type NotificationSnackbarProps = {
    message: string,
    duration?: number,
    severity?: AlertColor,
    open?: boolean,
    setOpen: (a: boolean) => void
};

export default function NotificationSnackbar({ message, duration = 5000, severity = "info", open = false, setOpen }: NotificationSnackbarProps) {

    const handleClose = (event: React.SyntheticEvent | Event, reason?: string) => {
        if (reason === 'clickaway') {
            return;
        }

        setOpen(false);
    };

    return (
        <Snackbar
            open={open}
            autoHideDuration={duration}
            onClose={handleClose}
        >
            <Alert variant="filled" severity={severity} onClose={handleClose} >
                {message}
            </Alert>
        </Snackbar>
    );
}