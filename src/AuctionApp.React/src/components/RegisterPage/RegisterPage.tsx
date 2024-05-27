import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useApi } from "../../contexts/ApiContext";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { RegisterSchema } from "./RegisterFormTypes";
import RegisterFormField from "./RegisterFormField";
import { AxiosError } from "axios";

import './RegisterPage.css';

export default function RegisterPage() {

    const navigate = useNavigate();

    const api = useApi().api;

    const {
        register,
        handleSubmit,
        formState: { errors },
        setError,
    } = useForm<FormData>({
        resolver: zodResolver(RegisterSchema),
    });

    const onSubmit = async (data: FormData) => {

        api.register(data.email, data.password)
            .then(() => navigate("/"))
            .catch((error) => {

                if (error instanceof AxiosError) {

                    if (!error.response) {
                        setError("password", {
                            type: "server",
                            message: 'No connection to the server',
                        })
                    }

                    const msg = Object.values(error.response?.data.errors)[0];

                    setError("password", {
                        type: "server",
                        message: msg,
                    })

                }
            });
    }

    return (
        <div className='user-register'>
            <form className="user-register-content" onSubmit={handleSubmit(onSubmit)}>
                <h1 className="text-3xl font-bold mb-4">
                    Register
                </h1>
                <RegisterFormField
                    type="email"
                    placeholder="Email"
                    name="email"
                    register={register}
                    error={errors.email}
                />

                <RegisterFormField
                    type="password"
                    placeholder="Password"
                    name="password"
                    register={register}
                    error={errors.password}
                />

                <Button className="my-5" variant="outlined" color="primary" type="submit">Register</Button>

                <small className="flex items-baseline">Already have an account?
                    <Button href="/login"> Log In here</Button>
                </small>
            </form>
        </div>
    );
}