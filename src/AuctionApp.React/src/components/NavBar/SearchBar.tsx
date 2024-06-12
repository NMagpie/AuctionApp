import { IconButton } from "@mui/material";
import { Dispatch, SetStateAction, useState } from "react";
import SearchIcon from '@mui/icons-material/Search';
import CancelIcon from '@mui/icons-material/Cancel';

import './SearchBar.css';
import SearchBarInput from "./SearchBarInput";

type SearchBarProps = {
    isSearchBarOpen: boolean;
    setSearchBarOpen: Dispatch<SetStateAction<boolean>>;
}

export default function SearchBar({ isSearchBarOpen, setSearchBarOpen }: SearchBarProps) {

    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (e) => {
        setInputValue(e.target.value);
    };

    return (
        <>
            <div className={`flex sm:hidden ${isSearchBarOpen && 'grow'}`}>
                {isSearchBarOpen ?
                    <>
                        <IconButton
                            onClick={() => setSearchBarOpen(false)}
                            disableRipple={true}
                            sx={{
                                color: "white",
                                backgroundColor: "#bdbdbd"
                            }}>
                            <CancelIcon />

                        </IconButton>

                        <SearchBarInput inputValue={inputValue} handleInputChange={handleInputChange} />
                    </>
                    :
                    <IconButton
                        onClick={() => setSearchBarOpen(true)}
                        disableRipple={true}
                        sx={{
                            color: "white",
                            backgroundColor: "#bdbdbd"
                        }}>

                        <SearchIcon />

                    </IconButton>}
            </div>

            <div className="grow hidden sm:flex">
                <SearchBarInput inputValue={inputValue} handleInputChange={handleInputChange} />
            </div>
        </>
    );
}