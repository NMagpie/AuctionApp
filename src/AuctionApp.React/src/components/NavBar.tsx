import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import Container from '@mui/material/Container';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import MenuItem from '@mui/material/MenuItem';
import AccountBalanceIcon from '@mui/icons-material/AccountBalance';
import SearchIcon from '@mui/icons-material/Search';
import AddCardIcon from '@mui/icons-material/AddCard';

import '../styles/NavBar.css'
import { Link, useNavigate } from 'react-router-dom';
import { ApiContext, User } from '../contexts/ApiContext';
import { InputBase, Modal } from '@mui/material';
import { FC, useContext, useState } from 'react';
import { UserInfoModal, UserLoginModal, UserRegisterModal } from './UserModals';

const pages = [
    { pageName: "Home", path: "/" }
];

function NavBar() {
    return (
        <AppBar className='navbar'>
            <Container>
                <Toolbar>

                    <ToolBarLarge />

                    <ToolBarMobile />

                </Toolbar>
            </Container>
        </AppBar>
    );
}

const ToolBarLarge = () => {
    return (
        <div className='toolbar hidden md:flex'>

            <Link className='logo mr-2 flex' to={"/"}>
                <AccountBalanceIcon className='mr-2' />
                <Typography
                    variant="h6"
                    noWrap
                >
                    Auction App
                </Typography>
            </Link>

            <SearchBar />

            <UserPanel />

            {/* <Box className=''>
        {pages.map((page) => (
            <Button
                key={page.pageName}
                onClick={handleCloseNavMenu}
                sx={{ color: 'white', display: 'flex' }}
            >
                {page.pageName}
            </Button>
        ))}
    </Box> */}

        </div>
    );
}

const ToolBarMobile = () => {

    const [anchorElNav, setAnchorElNav] = useState<null | HTMLElement>(null);

    const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElNav(event.currentTarget);
    };

    const handleCloseNavMenu = () => {
        setAnchorElNav(null);
    };

    return (
        <div className='toolbar flex md:hidden'>

            <UserPanel />

            <Link className='logo flex mx-auto' to={"/"}>
                <AccountBalanceIcon className='mr-2' />
                <Typography
                    variant="h5"
                    noWrap
                >
                    Auction App
                </Typography>
            </Link>

            <Box>
                <IconButton
                    size="large"
                    aria-label="account of current user"
                    aria-controls="menu-appbar"
                    aria-haspopup="true"
                    onClick={handleOpenNavMenu}
                    color="inherit"
                >
                    <SearchIcon />
                </IconButton>
                <Menu
                    className='block'
                    id="menu-appbar"
                    anchorEl={anchorElNav}
                    anchorOrigin={{
                        vertical: 'top',
                        horizontal: 'left',
                    }}
                    keepMounted
                    transformOrigin={{
                        vertical: 'top',
                        horizontal: 'left',
                    }}
                    open={Boolean(anchorElNav)}
                    onClose={handleCloseNavMenu}
                >
                    {pages.map((page) => (
                        <MenuItem key={page.pageName} onClick={handleCloseNavMenu}>
                            <Typography textAlign="center">{page.pageName}</Typography>
                        </MenuItem>
                    ))}
                </Menu>
            </Box>
        </div>
    );
}

const SearchBar = () => {
    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (event) => {
        setInputValue(event.target.value);
    };

    return (
        <>
            <div className={`hidden md:flex search-bar ${inputValue ? 'search-bar-active' : ''}`}>
                <SearchIcon />
                <InputBase
                    className='grow'
                    placeholder='Search...'
                    value={inputValue}
                    onChange={handleInputChange} />
            </div>
            <div className={`flex md:hidden search-bar ${inputValue ? 'search-bar-active' : ''}`}>
                <SearchIcon />
                <InputBase
                    className='grow'
                    placeholder='Search...'
                    value={inputValue}
                    onChange={handleInputChange} />
            </div>
        </>
    );
}

const UserPanel = () => {

    const navigate = useNavigate();

    const refreshPage = () => {
        navigate(0);
    }

    const apiProvider = useContext(ApiContext);

    const [user] = useState(apiProvider.user);

    const [anchorElUser, setAnchorElUser] = useState<null | HTMLElement>(null);

    const [userInfoModal, setUserInfoModal] = useState(false);

    const [userRegisterModal, setUserRegisterModal] = useState(false);

    const [userLoginModal, setUserLoginModal] = useState(false);

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => setAnchorElUser(event.currentTarget);

    const handleCloseUserMenu = () => setAnchorElUser(null);

    const handleOpenUserInfo = () => setUserInfoModal(true);

    const handleCloseUserInfo = () => setUserInfoModal(false);

    const handleOpenUserRegister = () => setUserRegisterModal(true);

    const handleCloseUserRegister = () => setUserRegisterModal(false);

    const handleOpenUserLogin = () => setUserLoginModal(true);

    const handleCloseUserLogin = () => setUserLoginModal(false);

    const settings = [
        { title: 'Profile', action: () => handleOpenUserInfo() },
        { title: 'Logout', action: () => { apiProvider.logout(); refreshPage(); } }
    ];

    return (
        <>
            <div className='hidden md:flex items-center'>

                <UserInfoModal
                    isOpen={userInfoModal}
                    handleClose={handleCloseUserInfo}
                    user={user!}
                />

                <UserRegisterModal
                    isOpen={userRegisterModal}
                    handleClose={handleCloseUserRegister}
                    handleOpenLogin={handleOpenUserLogin}
                />

                <UserLoginModal
                    isOpen={userLoginModal}
                    handleClose={handleCloseUserLogin}
                    handleOpenRegister={handleOpenUserRegister}
                />

                {
                    user ?
                        <>

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
                        </> :
                        <>

                            <Box className='flex items-center'>

                                <Button
                                    onClick={handleOpenUserLogin}
                                    sx={{ color: 'white' }}
                                >
                                    Sign In
                                </Button>

                                <Button
                                    onClick={handleOpenUserRegister}
                                    sx={{ color: 'white' }}
                                >
                                    Sign Up
                                </Button>

                            </Box>
                        </>
                }
            </div>

            <div className='flex md:hidden items-center'>
                <Box className='flex items-center'>
                    <Tooltip title="Open settings">
                        <IconButton onClick={handleOpenUserMenu}>
                            <Avatar alt={user?.userName?.charAt(0)} src={user ? "./src" : ""} />
                        </IconButton>
                    </Tooltip>
                </Box>
            </div>
        </>
    );
}

export default NavBar;