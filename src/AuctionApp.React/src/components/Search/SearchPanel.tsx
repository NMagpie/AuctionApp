import { EProductSearchPresets, ProductsApiSearchProductsRequest as QueryBody } from "../../api/openapi-generated";
import { FormControl, InputLabel, Select, MenuItem, Button, TextField } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { getSearchQuery } from "../../common";
import { useState } from "react";

import './SearchPanel.css';

function SearchPanel({ query }: { query: QueryBody }) {

    const [selectedQuery, setSelectedQuery] = useState(query);

    const navigate = useNavigate();

    const navigateByQuery = () => {
        const searchQueryString = getSearchQuery(selectedQuery);

        navigate(searchQueryString);
    };

    const resetQuery = () => {
        const emptyQuery = {
            searchQuery: selectedQuery.searchQuery,
            searchPreset: "",
            category: "",
            minPrice: "",
            maxPrice: "",
        };

        setSelectedQuery(emptyQuery);

        const searchQueryString = getSearchQuery(emptyQuery);

        navigate(searchQueryString);
    };

    const handlePresetChange = (e) => {
        setSelectedQuery({
            ...selectedQuery,
            searchPreset: EProductSearchPresets[e.target.value] ?? "none",
        });
    };

    const handleCategoryChange = (e) => {
        setSelectedQuery({
            ...selectedQuery,
            category: e.target.value ?? "",
        });
    };

    const handleMinPriceChange = (e) => {
        setSelectedQuery({
            ...selectedQuery,
            minPrice: e.target?.value ?? 0,
        });
    };

    const handleMaxPriceChange = (e) => {
        setSelectedQuery({
            ...selectedQuery,
            maxPrice: e.target?.value ?? 0,
        });
    };

    const getValidPreset = () => selectedQuery?.searchPreset?.toString() === "none" ? "" : selectedQuery.searchPreset ?? "";

    return (
        <div className='search-page-panel'>
            <p className="font-bold text-lg my-1">
                Filters:
            </p>

            <FormControl fullWidth className="text-center">
                <InputLabel shrink>Relevance</InputLabel>
                <Select
                    value={getValidPreset()}
                    label="Relevance"
                    onChange={handlePresetChange}
                >
                    <MenuItem value="">None</MenuItem>
                    <MenuItem value={"ComingSoon"}>Coming Soon</MenuItem>
                    <MenuItem value={"EndingSoon"}>Ending Soon</MenuItem>
                    <MenuItem value={"MostActive"}>Most Active</MenuItem>
                    <MenuItem value={"BidHigh"}>Highest Bids</MenuItem>
                    <MenuItem value={"BidLow"}>Lowest Bids</MenuItem>
                </Select>
            </FormControl>

            <FormControl fullWidth className="text-center">
                <InputLabel shrink>Category</InputLabel>
                <Select
                    value={selectedQuery.category ?? ""}
                    label="Category"
                    onChange={handleCategoryChange}
                >
                    <MenuItem value="">None</MenuItem>
                    <MenuItem value={"Antiques"}>Antiques</MenuItem>
                    <MenuItem value={"Electronics"}>Electronics</MenuItem>
                    <MenuItem value={"Fashion"}>Fashion</MenuItem>
                    <MenuItem value={"Books"}>Books</MenuItem>
                    <MenuItem value={"Other"}>Other</MenuItem>
                </Select>
            </FormControl>

            <p className="font-bold text-lg my-1">
                Price
            </p>

            <div className="flex items-baseline">
                <TextField
                    label="Min"
                    type="number"
                    value={selectedQuery.minPrice || ""}
                    onChange={handleMinPriceChange}
                />
                <p className="mx-3">-</p>
                <TextField
                    label="Max"
                    type="number"
                    value={selectedQuery.maxPrice || ""}
                    onChange={handleMaxPriceChange}
                />
            </div>

            <div className="submit-buttons">
                <Button onClick={resetQuery}>Reset</Button>
                <Button onClick={navigateByQuery}>Apply</Button>
            </div>
        </div>
    )
}

export default SearchPanel;