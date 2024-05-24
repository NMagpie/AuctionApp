import { FieldError, UseFormRegister } from "react-hook-form";
import { z, ZodType } from "zod";

export type RegisterFormData = {
    email: string;
    password: string;
};

export type RegisterFormFieldProps = {
    type: string;
    placeholder: string;
    name: RegisterValidFieldNames;
    register: UseFormRegister<FormData>;
    error: FieldError | undefined;
    valueAsNumber?: boolean;
};

export type RegisterValidFieldNames =
    | "email"
    | "password";

const passwordValidation = new RegExp(
    /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{8,}$/
);

export const RegisterSchema: ZodType<RegisterFormData> = z
    .object({
        email: z.string().email(),
        password: z
            .string()
            .min(8, { message: "Password is too short" })
            .regex(passwordValidation, { message: "Password must contain at least one: lowercase, uppercase letters, number and special character" })
    });