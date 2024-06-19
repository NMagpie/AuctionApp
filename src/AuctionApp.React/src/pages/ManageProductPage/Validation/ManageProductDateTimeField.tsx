import { DateTimeField } from "@mui/x-date-pickers";
import { Typography } from "@mui/material";
import { ManageProductFormFieldProps } from "./ManageProductFormTypes";

const ManageProductDateTimeFormField: React.FC<ManageProductFormFieldProps> = ({
    label,
    name,
    register,
    error,
    defaultValue,
}) => (
    <div className="flex flex-col">
        <Typography className="mb-1">{label}</Typography>
        <DateTimeField
            variant="outlined"
            defaultValue={defaultValue}
            {...register(name, { valueAsDate: true })}
        />
        {error && <span className="error-message">{error.message}</span>}
    </div>
);

export default ManageProductDateTimeFormField;