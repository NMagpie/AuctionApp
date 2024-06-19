import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Button, MenuItem } from '@mui/material';
import { useApi } from '../../contexts/ApiContext';
import { useLoaderData, useNavigate } from 'react-router-dom';
import { AxiosError } from 'axios';
import { useSnackbar } from 'notistack';
import { categoryList } from '../../common';

import ManageProductCategorySelectFormField from './Validation/ManageProductCategorySelectFormField';
import ManageProductDateTimeFormField from './Validation/ManageProductDateTimeField';
import { ManageProductSchema } from './Validation/ManageProductFormTypes';
import ManageProductTextFormField from './Validation/ManageProductTextFormField';
import ManageProductTextMultilineFormField from './Validation/ManageProductTextMultilineFormField';
import { ProductDto } from '../../api/openapi-generated';
import dayjs from 'dayjs';

import './ManageProductPage.css';

function ManageProductPage() {

    const productData: ProductDto = useLoaderData()?.productData;

    const productExists = !!productData;

    const api = useApi();

    const navigate = useNavigate();

    const { enqueueSnackbar } = useSnackbar();

    const defaultValues = {
        title: productExists ? productData.title : "",
        description: productExists ? productData.description : "",
        startTime: productExists ? dayjs(productData.startTime) : dayjs(),
        endTime: productExists ? dayjs(productData.endTime) : dayjs(),
        initialPrice: productExists ? productData.initialPrice : "",
        category: productExists ? productData!.category!.name! : "",
    };

    const {
        register,
        handleSubmit,
        formState: { errors },
        setError,
    } = useForm<FormData>({
        resolver: zodResolver(ManageProductSchema),
    });


    const onSubmit = async (data: FormData) => {

        const invokeRequest = productExists ? api.products.updateProduct.bind(api.products) : api.products.createProduct.bind(api.products);

        const requestType = productExists ? "updateProductRequest" : "createProductRequest";

        const requestBody = {
            ...productExists && { id: productData.id },
            [requestType]: {
                title: data.title,
                description: data.description,
                startTime: data.startTime,
                endTime: data.endTime,
                initialPrice: data.initialPrice,
                category: data.category,
            }
        };

        invokeRequest(requestBody)
            .then((response) => navigate(`/products/${response.data.id}`))
            .catch((error) => {

                if (error instanceof AxiosError) {

                    if (!error.response) {
                        enqueueSnackbar("Cannot connect to server... Try again later", {
                            variant: "error"
                        });
                    }

                    let msg = 'Unexpected Error';

                    if (error.response?.data.errors) {
                        msg = Object.values(error.response?.data.errors)[0]
                    }

                    if (error.response?.data.Message) {
                        msg = error.response?.data.Message;
                    }

                    enqueueSnackbar(msg, {
                        variant: "error"
                    });

                }
            });
    }

    return (
        <div className='create-product-body'>
            <h1>{`${productExists ? "Edit" : "Create"} Product`}</h1>

            <form className='create-product-form' onSubmit={handleSubmit(onSubmit)}>

                <ManageProductTextFormField
                    type="text"
                    placeholder="Title"
                    name="title"
                    register={register}
                    error={errors.title}
                    defaultValue={defaultValues.title}
                />

                <ManageProductTextMultilineFormField
                    type="text"
                    placeholder="Description"
                    name="description"
                    register={register}
                    error={errors.description}
                    defaultValue={defaultValues.description}
                />

                <ManageProductTextFormField
                    type="number"
                    placeholder="Initial Price"
                    name="initialPrice"
                    register={register}
                    error={errors.initialPrice}
                    valueAsNumber
                    defaultValue={defaultValues.initialPrice}
                />

                <ManageProductDateTimeFormField
                    label="Start Time"
                    name="startTime"
                    register={register}
                    error={errors.startTime}
                    defaultValue={defaultValues.startTime}
                />

                <ManageProductDateTimeFormField
                    label="End Time"
                    name="endTime"
                    register={register}
                    error={errors.endTime}
                    defaultValue={defaultValues.endTime}
                />

                <ManageProductCategorySelectFormField
                    placeholder="Category"
                    name="category"
                    register={register}
                    error={errors.category}
                    defaultValue={defaultValues.category}
                >
                    {categoryList.map(c => <MenuItem key={`category-${c}`} value={c}>{c}</MenuItem>)}
                </ManageProductCategorySelectFormField>

                <Button
                    className="submit-button"
                    variant="outlined"
                    color="primary"
                    type="submit"
                >{productExists ? "Done" : "Create"}</Button>
            </form>
        </div>
    )
}

export default ManageProductPage;