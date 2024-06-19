import { Tooltip, IconButton } from '@mui/material';
import { useState } from 'react';
import { ProductDto } from '../../api/openapi-generated';
import { productDtoToProduct } from '../../common';
import { useApi } from '../../contexts/ApiContext';
import SearchResultCard from '../Search/SearchResultCard';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { useNavigate } from 'react-router-dom';
import { useSnackbar } from 'notistack';

import './UserProductList.css';

function UserProductList({ products, editMode }) {

    const [productsState, setProductsState] = useState(products);

    const { enqueueSnackbar } = useSnackbar();

    const navigate = useNavigate();

    const api = useApi();

    const isProductEditable = (product: ProductDto): boolean => {
        const editableTime = Date.parse(product.startTime) - 5 * 60_000;

        return Date.now() < editableTime;
    };

    const deleteProduct = async (productId: number, isEditable) => {

        if (!isEditable) { showEditError(); return; }

        try {
            await api.products.deleteProduct({ id: productId });

            const updatedProducts = productsState.filter((p: ProductDto) => p.id !== productId);

            setProductsState(updatedProducts);

            enqueueSnackbar("Product has been deleted", {
                variant: "info"
            });
        } catch (e) {
            showEditError();
        }
    };

    const editProduct = (productId: number, isEditable) => {
        if (!isEditable) { showEditError(); return; }

        navigate(`/edit-product/${productId}`);
    };

    const showEditError = () => {
        enqueueSnackbar("Cannot edit or delete products 5 minutes before its selling start", {
            variant: "error"
        });
    };

    return (
        productsState.length ?
            <>
                <div className="user-product-list">
                    {productsState.map((product: ProductDto) => {

                        const isEditable = isProductEditable(product);

                        return (
                            <div className="relative" key={`user-product-${product.id}`}>

                                {editMode &&
                                    <>
                                        <Tooltip
                                            className="bg-slate-700 text-white"
                                            onClick={() => deleteProduct(product.id, isEditable)}
                                            title="Delete"
                                        >
                                            <IconButton
                                                className={`product-card-edit-button right-1 ${isEditable ? "text-white" : "text-slate-400"}`} >
                                                <DeleteIcon />
                                            </IconButton>
                                        </Tooltip>

                                        <Tooltip
                                            className="bg-slate-700 text-white"
                                            onClick={() => editProduct(product.id, isEditable)}
                                            title="Edit"
                                        >
                                            <IconButton
                                                className={`product-card-edit-button right-12 ${isEditable ? "text-white" : "text-slate-400"}`}>
                                                <EditIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </>
                                }

                                <SearchResultCard
                                    product={productDtoToProduct(product)}
                                />

                            </div>
                        );
                    }
                    )}
                </div>
            </>
            :
            <h2>User hasn't created any of products...</h2>
    );
}

export default UserProductList;