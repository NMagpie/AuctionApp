import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useApi } from "../../contexts/ApiContext";
import { Avatar, Divider, Typography } from "@mui/material";
import { UserDto } from "../../api/openapi-generated";
import AuctionTimers from "./AuctionTimers";

import './ProductPage.css';

export type Product = {
    id: number;
    title: string;
    description: string;
    creator: UserDto | null;
    startTime: Date | null;
    endTime: Date | null;
};

export default function ProductPage() {

    const { id } = useParams();

    const [product, setProduct] = useState<Product | null>(null);

    const api = useApi().api;

    const getProduct = async () => {
        let { data } = await api.products.productsIdGet({ id: parseInt(id ?? "") });

        const product = {
            id: data.id ?? 0,
            title: data.title ?? '',
            description: data.description ?? '',
            creator: data.creator ?? null,
            startTime: data.startTime ? new Date(data.startTime) : null,
            endTime: data.endTime ? new Date(data.endTime) : null,
        };

        const date = new Date();

        date.setDate(date.getDate() + 2);

        product.endTime = date;

        setProduct(product);
    };

    useEffect(() => {
        getProduct();
    }, []);

    return (
        <div className="product-card">

            <div className="product-info">
                <img src="https://bidpro.webdevia.com/wp-content/uploads/2018/05/alexander-andrews-BX4Q0gojWAs-unsplash.jpg" alt={`product-${product?.id}-img`}></img>

                <Typography className="product-title hidden lg:inline" variant="h2">{product?.title}</Typography>
                <Typography className="product-title lg:hidden" variant="h3">{product?.title}</Typography>

                <Typography className="font-bold">{product?.description}</Typography>
            </div>

            <div className="product-brief">
                <Typography className="product-brief-title hidden lg:block" variant="h3">{product?.title?.substring(0, 50)}</Typography>
                <Typography className="product-brief-title lg:hidden" variant="h4">{product?.title?.substring(0, 50)}</Typography>

                <div className="flex flex-row items-baseline">
                    <Typography variant="h6" className="mr-3">Creator:</Typography>

                    <Link className="flex flex-row" to={`/users/${product?.creator?.id}`}>
                        <Avatar className="mr-2" alt={product?.creator?.userName?.charAt(0)} src="./src" />
                        <Typography variant="h6">{product?.creator?.userName}</Typography>
                    </Link>
                </div>

                <div className="product-brief-divider"/>

                <AuctionTimers product={product} />

            </div>

        </div>
    )
}