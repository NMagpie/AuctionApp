import { useLoaderData } from "react-router-dom";
import { useApi } from "../../contexts/ApiContext";
import { Avatar, Typography } from "@mui/material";
import Divider from '@mui/material/Divider';
import { ProductDto } from "../../api/openapi-generated";
import { productDtoToProduct } from "../../common";
import SearchResultCard from "../../components/Search/SearchResultCard";

import './UserPage.css';

export default function UserPage() {

    const { user, userProductsData } = useLoaderData();

    const { api } = useApi();

    const userProductList = (products) => {
        return (
            products.length ?
                <>
                    <div className="user-product-list">
                        {products.map((product: ProductDto) => {
                            return (
                                <SearchResultCard
                                    key={`user-product-${product.id}`}
                                    product={productDtoToProduct(product)}
                                />
                            );
                        }
                        )}
                    </div>
                </>
                :
                <h2>User hasn't created any of products...</h2>
        );
    }

    return (
        <div className="user-page-info">

            <div className="user-page-info-content">
                <div className="flex flex-row items-center">
                    <Avatar sx={{ width: 50, height: 50, fontSize: 35 }} className="mr-5" alt={user?.userName ?? ""} src="./src" />
                    <Typography variant="h5">{user?.userName}</Typography>
                </div>

                <Divider className="bg-black border-slate-700 border-solid w-1/2" />

                <Typography variant="h5">Products:</Typography>

                {userProductList(userProductsData.items)}
            </div>

        </div>
    )
}