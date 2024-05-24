import { AccountBalance } from "@mui/icons-material";
import { AppBar, Container, Toolbar, Typography } from "@mui/material";
import { Link } from "react-router-dom";

import './NavBar.css';
import SearchBar from "./SearchBar";
import UserInfo from "./UserInfo";

export default function NavBar() {
    return (
        <AppBar className='navbar'>
            <Container>
                <Toolbar>

                    <Link className='logo' to={"/"}>
                        <AccountBalance className='mr-2' />
                        <Typography
                            variant="h6"
                            noWrap
                        >
                            Auction App
                        </Typography>
                    </Link>

                    <SearchBar />

                    <UserInfo />

                </Toolbar>
            </Container>
        </AppBar>
    );
}