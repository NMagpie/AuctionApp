import { IconButton } from "@mui/material";
import { Dispatch, SetStateAction } from "react";
import SearchIcon from '@mui/icons-material/Search';
import CancelIcon from '@mui/icons-material/Cancel';
import SearchBarInput from "./SearchBarInput";

type SearchBarProps = {
    isSearchBarOpen: boolean;
    setSearchBarOpen: Dispatch<SetStateAction<boolean>>;
}

export default function SearchBar({ isSearchBarOpen, setSearchBarOpen }: SearchBarProps) {

    return (
        <div className={`flex sm:grow ${isSearchBarOpen && "grow"}`}>

            {isSearchBarOpen &&
                <IconButton
                    className="sm:hidden"
                    onClick={() => setSearchBarOpen(false)}
                    disableRipple={true}
                    sx={{
                        color: "white",
                        backgroundColor: "#bdbdbd"
                    }}>
                    <CancelIcon />

                </IconButton>}

            <div className={`grow sm:block ${isSearchBarOpen ? "block" : "hidden"}`}>
                <SearchBarInput />
            </div>

            {!isSearchBarOpen &&
                <IconButton
                    className="sm:hidden"
                    onClick={() => setSearchBarOpen(true)}
                    disableRipple={true}
                    sx={{
                        color: "white",
                        backgroundColor: "#bdbdbd"
                    }}>

                    <SearchIcon />

                </IconButton>}
        </div>
    );
}