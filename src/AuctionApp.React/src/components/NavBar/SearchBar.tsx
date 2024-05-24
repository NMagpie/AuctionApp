import { IconButton, InputBase } from "@mui/material";
import { useState } from "react";
import SearchIcon from '@mui/icons-material/Search';

import './SearchBar.css';

export default function SearchBar() {
    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (event) => {
        setInputValue(event.target.value);
    };

    return (
        <div className="grow">
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
        </div>
    );
}