import { Select, Typography } from "@mui/material";
import { CreateProductFormFieldProps } from "./CreateProductFormTypes";

const CreateProductCategorySelectFormField: React.FC<CreateProductFormFieldProps> = ({
    placeholder,
    name,
    register,
    error,
    valueAsNumber,
    children,
}) => (
    <div className="flex flex-col">
        <Typography className="mb-1">{placeholder}</Typography>
        <Select
            defaultValue=""
            placeholder={placeholder}
            {...register(name, { valueAsNumber })}
        >
            {children}
        </Select>
        {error && <span className="error-message">{error.message}</span>}
    </div>
);

export default CreateProductCategorySelectFormField;