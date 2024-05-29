import { IconButton, InputBase } from "@mui/material";
import { Dispatch, SetStateAction, useState } from "react";
import SearchIcon from '@mui/icons-material/Search';
import CancelIcon from '@mui/icons-material/Cancel';

import './SearchBar.css';

interface IProps {
    isSearchBarOpen: boolean;
    setSearchBarOpen: Dispatch<SetStateAction<boolean>>;
}

export default function SearchBar({ isSearchBarOpen, setSearchBarOpen }: IProps) {
    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (event) => {
        setInputValue(event.target.value);
    };

    const SearchBarBody = () => {
        return (
            <div className={`search-bar ${inputValue ? 'search-bar-active' : ''}`}>

                <InputBase
                    className='grow px-3'
                    placeholder='Search...'
                    value={inputValue}
                    onChange={handleInputChange}
                />
                <IconButton>
                    <SearchIcon />
                </IconButton>
            </div>
        );
    };

    return (
        <>

            <div className={`flex sm:hidden ${isSearchBarOpen ? 'grow' : ''}`}>
                {isSearchBarOpen ?
                    <>
                        <IconButton onClick={() => setSearchBarOpen(false)} disableRipple={true} sx={{ color: "white", backgroundColor: "#bdbdbd" }}>
                            <CancelIcon />
                        </IconButton>
                        <SearchBarBody />
                    </>
                    :
                    <IconButton onClick={() => setSearchBarOpen(true)} disableRipple={true} sx={{ color: "white", backgroundColor: "#bdbdbd" }}>
                        <SearchIcon />
                    </IconButton>}
            </div>

            <div className="grow hidden sm:flex">
                <SearchBarBody />
            </div>
        </>
    );
}