import { InputBase, IconButton } from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import SearchRecordsList from "./SearchRecordsList";
import { useApi } from "../../contexts/ApiContext";
import { SearchRecordDto } from "../../api/openapi-generated";

type SearchBarInputProps = {
    inputValue: string,
    handleInputChange: (e: any) => void,
};

export default function SearchBarInput({ inputValue, handleInputChange }: SearchBarInputProps) {
    const api = useApi();

    const navigate = useNavigate();

    const [searchRecords, setSearchRecords] = useState<SearchRecordDto[]>([]);

    const [focused, setFocused] = useState(false);
    const onFocus = () => setFocused(true);
    const onBlur = () => setFocused(false);

    const search = (e) => {
        e.preventDefault();

        setFocused(false);

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
            className={`search-bar ${inputValue && 'search-bar-active'} ${focused && searchRecords.length ? "rounded-t-md" : "rounded-md"}`}
        >

            <SearchRecordsList
                focused={focused}
                searchRecords={searchRecords}
            />

            <InputBase
                onFocus={onFocus}
                onBlur={onBlur}
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