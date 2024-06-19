import { categoryList, productDtoToProduct } from "../../common";
import { useApi } from "../../contexts/ApiContext";
import CategoryItem from "../../components/Home/CategoryItem";

import './HomePage.css';
import { useLoaderData, useNavigate } from "react-router-dom";
import SearchResultCard from "../../components/Search/SearchResultCard";
import { ProductDto } from "../../api/openapi-generated";
import { Button } from "@mui/material";

function HomePage() {

    const api = useApi();

    const { upcomingProducts, mostActiveProducts } = useLoaderData();

    const navigate = useNavigate();

    const cardList = (products, prefix, moreButtonAction) => {
        return (
            products.length ?
                <>
                    <div className="list-content">
                        {products.map((product: ProductDto) => {
                            return (
                                <SearchResultCard
                                    key={`${prefix}-${product.id}`}
                                    product={productDtoToProduct(product)}
                                />
                            );
                        }
                        )}
                    </div>
                    <Button
                        className="more-button"
                        onClick={moreButtonAction}
                    >More...</Button>
                </>
                :
                <h2>Sorry, nothing's here!</h2>
        );
    }

    return (
        <div className="home-page-body">
            <h1 className="home-title hidden sm:block">Hello{api.userIdentity && `, ${api.user?.userName}`}!</h1>
            <h2 className="home-title sm:hidden">Hello{api.userIdentity && `, ${api.user?.userName}`}!</h2>

            <div className="category-list">
                {categoryList.map(c => <CategoryItem key={`category-item-${c}`} category={c} />)}
            </div>

            <div className="recommend-products">
                <div className="recommend-list-body">
                    <h2>Coming Soon</h2>
                    {cardList(upcomingProducts, "upcoming", () => navigate("/search?preset=ComingSoon"))}
                </div>
                <div className="recommend-list-body">
                    <h2>Most Popular</h2>
                    {cardList(mostActiveProducts, "most-active", () => navigate("/search?preset=MostActive"))}
                </div>
            </div>
        </div>
    );
}

export default HomePage;