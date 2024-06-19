import { TextField, Typography } from "@mui/material";
import { ManageProductFormFieldProps } from "./ManageProductFormTypes";

const ManageProductTextFormField: React.FC<ManageProductFormFieldProps> = ({
    type,
    placeholder,
    name,
    register,
    error,
    valueAsNumber,
    defaultValue,
}) => (
    <div className="flex flex-col">
        <Typography className="mb-1">{placeholder}</Typography>
        <TextField
            type={type}
            variant="outlined"
            placeholder={placeholder}
            defaultValue={defaultValue}
            {...register(name, { valueAsNumber })}
        />
        {error && <span className="error-message">{error.message}</span>}
    </div>
);

export default ManageProductTextFormField;