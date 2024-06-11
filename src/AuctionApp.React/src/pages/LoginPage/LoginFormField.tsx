import { TextField } from "@mui/material";
import { LoginFormFieldProps } from "./LoginFormTypes";

const LoginFormField: React.FC<LoginFormFieldProps> = ({
    type,
    placeholder,
    name,
    register,
    error,
    valueAsNumber,
}) => (
    <>
        <TextField
            className="mt-3"
            type={type}
            placeholder={placeholder}
            {...register(name, { valueAsNumber })}
        />
        {error && <span className="error-message">{error.message}</span>}
    </>
);
export default LoginFormField;