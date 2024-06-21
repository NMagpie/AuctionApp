import { Box, Typography, Tooltip, IconButton, Avatar, Menu, MenuItem, Button } from "@mui/material";
import { useEffect, useState } from "react";
import { useApi } from "../../contexts/ApiContext";
import AddCardIcon from '@mui/icons-material/AddCard';
import { useLocation, useNavigate } from "react-router-dom";
import AddBalanceDialog from "./AddBalanceDialog";

import './UserInfo.css';

export default function UserInfo() {

    const navigate = useNavigate();

    const { pathname } = useLocation();

    const api = useApi();

    const [anchorElUser, setAnchorElUser] = useState<null | HTMLElement>(null);

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => setAnchorElUser(event.currentTarget);

    const handleCloseUserMenu = () => setAnchorElUser(null);

    const [addBalanceOpen, setAddBalanceOpen] = useState(false);

    const [user, setUser] = useState(api.user);

    useEffect(() => {
        setUser(api.user);
    }, [api.user]);

    useEffect(() => {
        api.getCurrentUser();
    }, [pathname])

    const settings = [
        { title: 'My Profile', action: () => { navigate("/me"); window.scrollTo(0, 0); } },
        { title: 'Create Product', action: () => { navigate("/create-product"); window.scrollTo(0, 0); } },
        { title: 'Logout', action: () => { api.logout(); navigate(0); } },
    ];

    return (
        <div className='user-info items-center'>

            {
                user ?

                    <Box className='flex items-center'>

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

                            <div className="menu-user-info">

                                <Typography className="menu-user-name">
                                    {user.userName}
                                </Typography>

                                <div>
                                    <div className="user-balance">
                                        <Typography>
                                            {user.balance} $
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

                                    </div>

                                    <Typography>
                                        {user.reservedBalance} $ (reserved)
                                    </Typography>

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