import { DateTimeField } from "@mui/x-date-pickers";
import { Typography } from "@mui/material";
import dayjs from "dayjs";
import { CreateProductFormFieldProps } from "./CreateProductFormTypes";

const CreateProductDateTimeFormField: React.FC<CreateProductFormFieldProps> = ({
    label,
    name,
    register,
    error,
}) => (
    <div className="flex flex-col">
        <Typography className="mb-1">{label}</Typography>
        <DateTimeField
            defaultValue={dayjs()}
            variant="outlined"
            {...register(name, { valueAsDate: true })}
        />
        {error && <span className="error-message">{error.message}</span>}
    </div>
);

export default CreateProductDateTimeFormField;