import { FC, useContext, useState } from "react";
import { ApiContext, User } from "../contexts/ApiContext";
import { Button, Modal, Stack, TextField, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { AxiosError } from "axios";

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

const passwordRegex = new RegExp('^(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z])(?=.*[\W_]).{8,}$');

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

    const [email, setEmail] = useState('')

    const [emailError, setEmailError] = useState(false)

    const [password, setPassword] = useState('')

    const [passwordErrorText, setPasswordErrorText] = useState("")

    const [passwordError, setPasswordError] = useState(false)

    const setPasswordErrorMessage = (msg: string) => {
        setPasswordError(!!msg);
        setPasswordErrorText(msg);
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        setEmailError(false)
        setPasswordError(false)

        if (email == '') {
            setEmailError(true)
        }

        switch (true) {
            case (password == ''):
                setPasswordError(true);
                return;
            case (password.length < 8):
                setPasswordErrorMessage('Password cannot be less than 8 characters long');
                return;
            case (password == password.toLowerCase() || password == password.toUpperCase()):
                setPasswordErrorMessage('Password must contain at least one uppercase and one lowercase letter');
                return;
            case (!passwordRegex.test(password)):
                setPasswordErrorMessage('Password must contain at least one number and one special character');
                return;
            default:
                setPasswordErrorMessage('');
        }

        if (email && password) {
            apiProvider.identity.identityRegisterPost({ registerRequest: { email: email, password: password } })
                .then((x) => {
                    handleClose();
                    refreshPage();
                })
                .catch(error => {

                    console.log(error);
                });
        }
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
                        error={emailError}
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
                        error={passwordError}
                        helperText={passwordErrorText}
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

    const [emailError, setEmailError] = useState(false)

    const [password, setPassword] = useState("")

    const [passwordError, setPasswordError] = useState(false)

    const handleSubmit = async (event) => {
        event.preventDefault()

        setEmailError(false)
        setPasswordError(false)

        if (email == '') {
            setEmailError(true)
        }

        if (password == '') {
            setPasswordError(true);
        }

        if (email && password) {
            apiProvider.login(email, password)
                .then(() => {
                    handleClose();
                    refreshPage();
                })
                .catch(error => {
                    console.log(error);
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