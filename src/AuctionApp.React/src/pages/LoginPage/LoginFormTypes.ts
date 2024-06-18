import { FieldError, UseFormRegister } from "react-hook-form";
import { z, ZodType } from "zod";

export type LoginFormData = {
    email: string;
    password: string;
};

export type LoginFormFieldProps = {
    type: string;
    placeholder: string;
    name: LoginValidFieldNames;
    register: UseFormRegister<FormData>;
    error: FieldError | undefined;
    valueAsNumber?: boolean;
};

export type LoginValidFieldNames =
    | "email"
    | "password";

export const LoginSchema: ZodType<LoginFormData> = z
    .object({
        email: z.string().email(),
        password: z.string(),
    });