import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useApi } from "../../contexts/ApiContext";
import { Avatar, Typography } from "@mui/material";

import './ProductPage.css';
import { UserDto } from "../../api/openapi-generated";

type Product = {
    id: number;
    title: string;
    description: string;
    creator: UserDto | null;
    startTime: Date | null;
    endTime: Date | null;
};

export default function ProductPage() {

    const { id } = useParams();

    const [Product, setProduct] = useState<Product | null>(null);

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

        setProduct(product);
    };

    console.log();

    useEffect(() => {
        getProduct();
    }, []);

    return (
        <div className="product-info">
            <div className="product-image">
                <img src="../../public/a.png" alt="placeholder"></img>
            </div>

            <Typography className="font-bold" variant="h2">{Product?.title}</Typography>

            <Typography className="font-bold" variant="h4">{Product?.description}</Typography>

            <div className="flex flex-row items-center">
                <Typography variant="h6" className="mr-5">Creator:</Typography>

                <Link className="flex flex-row items-center" to={`/users/${Product?.creator?.id}`}>
                    <Avatar className="mr-2" alt={Product?.creator?.userName?.charAt(0)} src="./src" />
                    <Typography variant="h6">{Product?.creator?.userName}</Typography>
                </Link>

            </div>

            <Typography variant="h6">Start Time: {Product?.startTime?.toLocaleString()}</Typography>
            <Typography variant="h6">End Time: {Product?.endTime?.toLocaleString()}</Typography>
        </div>
    )
}