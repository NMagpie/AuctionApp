import { Box, Typography, Tooltip, IconButton, Avatar, Menu, MenuItem, Button } from "@mui/material";
import { useEffect, useState } from "react";
import { useApi } from "../../contexts/ApiContext";
import AddCardIcon from '@mui/icons-material/AddCard';
import { useNavigate } from "react-router-dom";
import { User } from "../../api/ApiManager";
import AddBalanceDialog from "./AddBalanceDialog";

import './UserInfo.css';

export default function UserInfo() {

    const navigate = useNavigate();

    const { api, didUserLoad, user } = useApi();

    const [userState, setUser] = useState<User | null>(api.user);

    useEffect(() => {
        setUser(user);
    }, [didUserLoad, user]);

    const [anchorElUser, setAnchorElUser] = useState<null | HTMLElement>(null);

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => setAnchorElUser(event.currentTarget);

    const handleCloseUserMenu = () => setAnchorElUser(null);

    const [addBalanceOpen, setAddBalanceOpen] = useState(false);

    const settings = [
        { title: 'My Profile', action: () => { } },
        { title: 'Create Product', action: () => { navigate("/create-product"); } },
        { title: 'Logout', action: () => { api.logout(); navigate(0); } },
    ];

    return (
        <div className='user-info items-center'>

            {
                userState ?

                    <Box className='flex items-center'>

                        <Tooltip title="Open settings">
                            <IconButton onClick={handleOpenUserMenu}>
                                <Avatar alt={userState.userName?.charAt(0)} src="./src" />
                            </IconButton>
                        </Tooltip>

                        <Menu
                            sx={{ mt: '45px' }}
                            id="menu-appbar"
                            anchorEl={anchorElUser}
                            anchorOrigin={{
                                vertical: 'top',
                                horizontal: 'left',
                            }}
                            keepMounted
                            transformOrigin={{
                                vertical: 'top',
                                horizontal: 'left',
                            }}
                            open={Boolean(anchorElUser)}
                            onClose={handleCloseUserMenu}
                        >

                            <div className="menu-user-info">

                                <Typography className="menu-user-name">
                                    {userState.userName}
                                </Typography>

                                <div key="user-balance" className='user-balance'>
                                    <Typography>
                                        {userState.balance} $
                                    </Typography>
                                    <Tooltip title="Add balance">
                                        <IconButton
                                            size="large"
                                            onClick={() => { setAddBalanceOpen(true); }}
                                            color="inherit"
                                        >
                                            <AddCardIcon />
                                        </IconButton>
                                    </Tooltip>
                                    <AddBalanceDialog
                                        open={addBalanceOpen}
                                        onClose={() => { setAddBalanceOpen(false); }}
                                    />
                                </div>

                                <div className="text-flex-divider" />

                            </div>

                            {settings.map((setting) => (
                                <MenuItem key={setting.title} onClick={() => { handleCloseUserMenu(); setting.action() }}>
                                    <Typography textAlign="center">{setting.title}</Typography>
                                </MenuItem>
                            ))}
                        </Menu>

                    </Box>
                    :
                    <>
                        <IconButton className="flex md:hidden" onClick={() => navigate("/login")}>
                            <Avatar alt={""} src="./src" />
                        </IconButton>

                        <div className='guest-panel'>

                            <Button
                                href="/login"
                            >
                                Sign In
                            </Button>

                            <Button
                                href="/register"
                            >
                                Sign Up
                            </Button>

                        </div>
                    </>
            }
        </div>

    );
}