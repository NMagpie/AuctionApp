import { InputBase, IconButton } from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';
import { useNavigate } from "react-router-dom";

type SearchBarInputProps = {
    inputValue: string,
    handleInputChange: (e: any) => void,
};

export default function SearchBarInput({ inputValue, handleInputChange }: SearchBarInputProps) {
    const navigate = useNavigate();

    const search = (e) => {
        e.preventDefault();

        navigate(`/search?q=${inputValue}`);
    };

    return (
        <form
            className={`search-bar ${inputValue && 'search-bar-active'}`}
        >

            <InputBase
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