import { InputBase, IconButton } from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import SearchRecordsList from "./SearchRecordsList";
import { useApi } from "../../contexts/ApiContext";
import { SearchRecordDto } from "../../api/openapi-generated";
import useComponentVisible from "../../hooks/useComponentVisible";

import './SearchBarInput.css';

function SearchBarInput() {

    const api = useApi();

    const navigate = useNavigate();

    const { ref, isComponentVisible, setIsComponentVisible } = useComponentVisible(false);

    const [searchRecords, setSearchRecords] = useState<SearchRecordDto[]>([]);

    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (e) => {
        setInputValue(e.target.value);
    };

    const search = (e) => {
        e.preventDefault();

        setIsComponentVisible(false);

        navigate(`/search?q=${inputValue}`);

        if (inputValue && !searchRecords.some(r => r.searchQuery === inputValue)) {
            setSearchRecords([
                {
                    searchQuery: inputValue,
                },
                ...searchRecords.length >= 10 ?
                    [...searchRecords.slice(0, -1)] :
                    searchRecords,
            ]);
        }
    };

    const fetchSearchRecords = async () => {
        const { data } = await api.searchRecords.getRecentSearches();

        setSearchRecords(data.items);
    };

    useEffect(() => {
        if (api.userIdentity) {
            fetchSearchRecords();
        }
    }, [api.userIdentity]);

    return (
        <form
            ref={ref}
            className={`search-bar 
                ${inputValue && 'search-bar-active'} 
                ${isComponentVisible && searchRecords.length ? "rounded-t-md" : "rounded-md"}`}
        >

            <SearchRecordsList
                focused={isComponentVisible}
                searchRecords={searchRecords}
                setSearchRecords={setSearchRecords}
                setIsComponentVisible={setIsComponentVisible}
            />

            <InputBase
                onFocus={() => setIsComponentVisible(true)}
                className='grow px-3'
                placeholder='Search...'
                value={inputValue}
                onChange={handleInputChange}
            />

            <IconButton
                type="submit"
                onClick={search}
            >
                <SearchIcon />

            </IconButton>
        </form>
    );
}

export default SearchBarInput;