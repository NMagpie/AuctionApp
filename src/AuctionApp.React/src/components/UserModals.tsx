import { FC } from "react";
import { User } from "../contexts/ApiContext";
import { Modal, Typography } from "@mui/material";

interface UserInfoModalProps {
    isOpen: boolean;
    handleClose: () => void;
    user: User;
}

export const UserInfoModal: FC<UserInfoModalProps> = ({
    isOpen,
    handleClose,
    user
}) => {
    return (
        <Modal
            open={isOpen}
            onClose={handleClose}
        >
            <div className='user-modal'>
                <div className='user-modal-content'>
                    <Typography variant="h5" component="h2">
                        Profile
                    </Typography>
                </div>
            </div>
        </Modal>
    );
}