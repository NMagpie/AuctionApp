import { Card, CardActionArea, CardMedia, CardContent } from '@mui/material';
import CountDown from '../CountDown/CountDown';
import { useState } from 'react';
import { Product, ProductStatus, getProductStatus } from '../../common';
import { useNavigate } from 'react-router-dom';

import './SearchResultCard.css';

export default function SearchResultCard({ product }: { product: Product }) {

    const navigate = useNavigate();

    const navigateToProduct = () => navigate(`/products/${product.id}`);

    const [productStatus, setProductStatus] = useState<ProductStatus>(getProductStatus(product));

    const [cardHovered, setCardHovered] = useState(false);

    const getProductStatusText = () => {
        switch (productStatus) {
            case "pending":
                return <div className='flex flex-row'>
                    <p className='text-left my-0 bg-slate-300 rounded-md px-4'>Coming soon</p>
                    <p className='grow my-0'>Initial price: ${product.initialPrice}</p>
                </div>
            case "started":
                return <>Initial price: ${product.initialPrice}</>;
            case 'finished':
                return "Finished";
        }
    };

    return (
        <Card
            className="search-result-card"
            onMouseOver={() => setCardHovered(true)}
            onMouseOut={() => setCardHovered(false)}
        >
            <CardActionArea
                className='overflow-hidden'
                onClick={navigateToProduct}
            >

                <CardMedia
                    className={`card-image ${cardHovered && "scale-125"}`}
                    component="img"
                    image="https://bidpro.webdevia.com/wp-content/uploads/2018/05/alexander-andrews-BX4Q0gojWAs-unsplash.jpg"
                    alt={`product-${product.id}`}
                />

                {productStatus != "finished" &&
                    <div className={`card-count-down ${cardHovered && 'translate-y-20'}`}>
                        <CountDown
                            key={productStatus === "started" ? "finish-timer" : "start-timer"}
                            setProductStatus={
                                productStatus === "started" ? () => { setProductStatus("finished") } : () => { setProductStatus("started") }
                            }
                            targetDate={productStatus === "started" ? product?.endTime : product?.startTime} />
                    </div>
                }

            </CardActionArea>

            <CardContent>

                <CardActionArea
                    className='w-fit mt-0 mb-2'
                    onClick={navigateToProduct}
                >
                    <h2 className='my-0'>
                        {product.title.length > 27 ? `${product.title.substring(0,27)}...` : product.title}
                    </h2>
                </CardActionArea>

                <h3 className='text-right mb-0 mt-0'>
                    {getProductStatusText()}
                </h3>

            </CardContent>

        </Card>
    );
}