import { AccountBalance } from "@mui/icons-material";
import { AppBar, Container, Toolbar, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import SearchBar from "./SearchBar";
import UserInfo from "./UserInfo";
import { useState } from "react";

import './NavBar.css';

export default function NavBar() {

    const [isSearchBarOpen, setSearchBarOpen] = useState(false);

    return (
        <AppBar className='navbar bg-slate-600' position="sticky">
            <Container>
                <Toolbar className="bg-slate-600">

                    <Link className={`logo ${isSearchBarOpen ? 'hidden' : ''} sm:flex`} to={"/"}>
                        <AccountBalance className='mr-2' />
                        <Typography
                            variant="h6"
                            noWrap
                        >
                            Auction App
                        </Typography>
                    </Link>

                    <SearchBar isSearchBarOpen={isSearchBarOpen} setSearchBarOpen={setSearchBarOpen} />

                    <div className={`${isSearchBarOpen ? 'hidden' : ''} sm:block`}>
                        <UserInfo />
                    </div>

                </Toolbar>
            </Container>
        </AppBar>
    );
}