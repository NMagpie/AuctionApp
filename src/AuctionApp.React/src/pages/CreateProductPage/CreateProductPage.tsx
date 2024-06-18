import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import CreateProductTextFormField from './Validation/CreateProductTextFormField';
import CreateProductTextMultilineFormField from './Validation/CreateProductTextMultilineFormField';
import { Button, MenuItem } from '@mui/material';
import { useApi } from '../../contexts/ApiContext';
import { useNavigate } from 'react-router-dom';
import { AxiosError } from 'axios';
import { CreateProductSchema } from './Validation/CreateProductFormTypes';
import CreateProductDateTimeFormField from './Validation/CreateProductDateTimeField';
import CreateProductCategorySelectFormField from './Validation/CreateProductCategorySelectFormField';
import { useSnackbar } from 'notistack';

import './CreateProductPage.css';

function CreateProductPage() {

    const { api } = useApi();

    const navigate = useNavigate();

    const { enqueueSnackbar } = useSnackbar();

    const {
        register,
        handleSubmit,
        formState: { errors },
        setError,
    } = useForm<FormData>({
        resolver: zodResolver(CreateProductSchema),
    });

    const onSubmit = async (data: FormData) => {

        api.products.createProduct({
            createProductRequest: {
                title: data.title,
                description: data.description,
                startTime: data.startTime,
                endTime: data.endTime,
                initialPrice: data.initialPrice,
                category: data.category,
            }
        })
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
            <h1>Create Product</h1>

            <form className='create-product-form' onSubmit={handleSubmit(onSubmit)}>

                <CreateProductTextFormField
                    type="text"
                    placeholder="Title"
                    name="title"
                    register={register}
                    error={errors.title}
                />

                <CreateProductTextMultilineFormField
                    type="text"
                    placeholder="Description"
                    name="description"
                    register={register}
                    error={errors.description}
                />

                <CreateProductTextFormField
                    type="number"
                    placeholder="Initial Price"
                    name="initialPrice"
                    register={register}
                    error={errors.initialPrice}
                    valueAsNumber
                />

                <CreateProductDateTimeFormField
                    label="Start Time"
                    name="startTime"
                    register={register}
                    error={errors.startTime}
                />

                <CreateProductDateTimeFormField
                    label="End Time"
                    name="endTime"
                    register={register}
                    error={errors.endTime}
                />

                <CreateProductCategorySelectFormField
                    placeholder="Category"
                    name="category"
                    register={register}
                    error={errors.category}
                >
                    <MenuItem value={"Antiques"}>Antiques</MenuItem>
                    <MenuItem value={"Electronics"}>Electronics</MenuItem>
                    <MenuItem value={"Fashion"}>Fashion</MenuItem>
                    <MenuItem value={"Books"}>Books</MenuItem>
                    <MenuItem value={"Other"}>Other</MenuItem>
                </CreateProductCategorySelectFormField>

                <Button
                    variant="outlined"
                    color="primary"
                    type="submit"
                >Create</Button>
            </form>
        </div>
    )
}

export default CreateProductPage;