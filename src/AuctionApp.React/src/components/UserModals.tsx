import { FC, useContext, useState } from "react";
import { ApiContext, User } from "../contexts/ApiContext";
import { Button, Modal, Stack, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

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

interface UserRegisterModalProps {
    isOpen: boolean;
    handleClose: () => void;
    handleOpenLogin: () => void;
}

export const UserRegisterModal: FC<UserRegisterModalProps> = ({
    isOpen,
    handleClose,
    handleOpenLogin
}) => {

    const navigate = useNavigate();

    const refreshPage = () => {
        navigate(0);
    }

    const apiProvider = useContext(ApiContext);

    // const [userName, setUserName] = useState('')
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')

    const handleSubmit = async (event) => {
        event.preventDefault();

        await apiProvider.identity.identityRegisterPost({ registerRequest: { email: email, password: password } });
    }

    return (
        <Modal
            open={isOpen}
            onClose={handleClose}
        >
            <div className='user-modal'>
                <form className='user-modal-content' onSubmit={handleSubmit}>

                    <Typography variant="h5" sx={{ mb: 2 }}>
                        Register
                    </Typography>

                    {/* <TextField
                            type="text"
                            variant='outlined'
                            color='primary'
                            label="User Name"
                            onChange={e => setUserName(e.target.value)}
                            value={userName}
                            fullWidth
                            required
                            sx={{ mb: 4 }}
                        /> */}

                    <TextField
                        type="email"
                        variant='outlined'
                        color='primary'
                        label="Email"
                        onChange={e => setEmail(e.target.value)}
                        value={email}
                        fullWidth
                        required
                        sx={{ mb: 4 }}
                    />

                    <TextField
                        type="password"
                        variant='outlined'
                        color='primary'
                        label="Password"
                        onChange={e => setPassword(e.target.value)}
                        value={password}
                        required
                        fullWidth
                        sx={{ mb: 4 }}
                    />

                    <Button variant="outlined" color="primary" type="submit">Register</Button>

                    <small className="my-4">Already have an account?
                        <Button onClick={() => { handleClose(); handleOpenLogin(); }}> Login here</Button>
                    </small>

                </form>

            </div>
        </Modal>
    );
}

interface UserLoginModalProps {
    isOpen: boolean;
    handleClose: () => void;
    handleOpenRegister: () => void;
}

export const UserLoginModal: FC<UserLoginModalProps> = ({
    isOpen,
    handleClose,
    handleOpenRegister
}) => {

    const navigate = useNavigate();

    const refreshPage = () => {
        navigate(0);
    }

    const apiProvider = useContext(ApiContext);

    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [emailError, setEmailError] = useState(false)
    const [passwordError, setPasswordError] = useState(false)

    const handleSubmit = async (event) => {
        event.preventDefault()

        setEmailError(false)
        setPasswordError(false)

        if (email == '') {
            setEmailError(true)
        }
        if (password == '') {
            setPasswordError(true)
        }

        if (email && password) {
            apiProvider.login(email, password)
                .then(() => {
                    handleClose();
                    refreshPage();
                })
                .catch(error => {

                });
        }
    }

    return (
        <Modal
            open={isOpen}
            onClose={handleClose}
        >
            <div className='user-modal'>
                <form className="user-modal-content" autoComplete="off" onSubmit={handleSubmit}>

                    <Typography variant="h5" sx={{ mb: 2 }}>
                        Log In
                    </Typography>

                    <TextField
                        label="Email"
                        onChange={e => setEmail(e.target.value)}
                        required
                        variant="outlined"
                        color="primary"
                        type="email"
                        sx={{ mb: 3 }}
                        fullWidth
                        value={email}
                        error={emailError}
                    />

                    <TextField
                        label="Password"
                        onChange={e => setPassword(e.target.value)}
                        required
                        variant="outlined"
                        color="primary"
                        type="password"
                        value={password}
                        error={passwordError}
                        fullWidth
                        sx={{ mb: 3 }}
                    />

                    <Button variant="outlined" color="primary" type="submit">Login</Button>

                    <small className="my-4">Need an account?
                        <Button onClick={() => { handleClose(); handleOpenRegister(); }}> Register here</Button>
                    </small>

                </form>
            </div>
        </Modal>
    );
}