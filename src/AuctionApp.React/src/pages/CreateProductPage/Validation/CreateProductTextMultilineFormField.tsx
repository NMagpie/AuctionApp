import { TextField, Typography } from "@mui/material";
import { CreateProductFormFieldProps } from "./CreateProductFormTypes";

const CreateProductTextMultilineFormField: React.FC<CreateProductFormFieldProps> = ({
    type,
    placeholder,
    name,
    register,
    error,
    valueAsNumber,
}) => (
    <div className="flex flex-col">
        <Typography className="mb-1">{placeholder}</Typography>
        <TextField
            type={type}
            multiline
            rows={4}
            placeholder={placeholder}
            {...register(name, { valueAsNumber })}
        />
        {error && <span className="error-message">{error.message}</span>}
    </div>
);

export default CreateProductTextMultilineFormField;