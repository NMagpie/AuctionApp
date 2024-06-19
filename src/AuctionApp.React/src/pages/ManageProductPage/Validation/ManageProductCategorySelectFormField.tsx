import { Select, Typography } from "@mui/material";
import { ManageProductFormFieldProps } from "./ManageProductFormTypes";

const ManageProductCategorySelectFormField: React.FC<ManageProductFormFieldProps> = ({
    placeholder,
    name,
    register,
    error,
    valueAsNumber,
    children,
    defaultValue,
}) => (
    <div className="flex flex-col">
        <Typography className="mb-1">{placeholder}</Typography>
        <Select
            placeholder={placeholder}
            defaultValue={defaultValue}
            {...register(name, { valueAsNumber })}
        >
            {children}
        </Select>
        {error && <span className="error-message">{error.message}</span>}
    </div>
);

export default ManageProductCategorySelectFormField;