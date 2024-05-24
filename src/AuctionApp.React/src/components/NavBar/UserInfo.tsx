import { Box, Typography, Tooltip, IconButton, Avatar, Menu, MenuItem, Button } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import { ApiContext, User } from "../../contexts/ApiContext";
import AddCardIcon from '@mui/icons-material/AddCard';
import { useNavigate } from "react-router-dom";

import './UserInfo.css';

export default function UserInfo() {

    const navigate = useNavigate();

    const apiProvider = useContext(ApiContext);

    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        setUser(apiProvider.user);
    }, [apiProvider.user]);

    const [anchorElUser, setAnchorElUser] = useState<null | HTMLElement>(null);

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => setAnchorElUser(event.currentTarget);

    const handleCloseUserMenu = () => setAnchorElUser(null);

    const settings = [
        { title: 'Profile', action: () => { } },
        { title: 'Logout', action: () => { apiProvider.logout(); navigate(0); } }
    ];

    return (
        <div className='user-info items-center'>

            {
                user ?

                    <Box className='flex items-center'>
                        <div className='hidden md:flex items-center'>
                            <Typography>
                                {user.balance} $
                            </Typography>
                            <Tooltip title="Add balance">
                                <IconButton
                                    size="large"
                                    aria-label="add balance of current user"
                                    aria-haspopup="true"
                                    onClick={() => { }}
                                    color="inherit"
                                >
                                    <AddCardIcon />
                                </IconButton>
                            </Tooltip>
                        </div>

                        <Tooltip title="Open settings">
                            <IconButton onClick={handleOpenUserMenu}>
                                <Avatar alt={user.userName?.charAt(0)} src="./src" />
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
                            {settings.map((setting) => (
                                <MenuItem key={setting.title} onClick={() => { handleCloseUserMenu(); setting.action() }}>
                                    <Typography textAlign="center">{setting.title}</Typography>
                                </MenuItem>
                            ))}
                        </Menu>

                    </Box>
                    :
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
            }
        </div>

    );
}