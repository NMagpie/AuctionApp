import { Button } from "@mui/material";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

import './CategoryItem.css';

function CategoryItem({ category }: { category: string }) {

    const [itemHovered, setItemHovered] = useState(false);

    const navigate = useNavigate();

    return (
        <Button
            className="category-item"
            onMouseOver={() => setItemHovered(true)}
            onMouseLeave={() => setItemHovered(false)}
            onClick={() => navigate(`/search?category=${category}&preset=ComingSoon`)}
        >
            <img
                className={itemHovered ? "scale-125" : ""}
                src={itemHovered ?
                    `./src/assets/${category}-Color-Category.png`
                    :
                    `./src/assets/${category}-Category.png`}
                alt={`category-${category}`}
            />
            <label className="mt-5">{category}</label>
        </Button>
    );
}

export default CategoryItem;