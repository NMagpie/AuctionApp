import { Button } from "@mui/material";
import { useContext } from "react";
import { useNavigate } from "react-router-dom";
import { ApiContext } from "../../contexts/ApiContext";

import './LoginPage.css';
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { LoginSchema } from "./LoginFormTypes";
import LoginFormField from "./LoginFormField";
import { AxiosError } from "axios";

export default function LoginPage() {
    const navigate = useNavigate();

    const apiProvider = useContext(ApiContext);

    const {
        register,
        handleSubmit,
        formState: { errors },
        setError,
    } = useForm<FormData>({
        resolver: zodResolver(LoginSchema),
    });

    const onSubmit = async (data: FormData) => {

        apiProvider.login(data.email, data.password)
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
        <div className='user-login'>
            <form className="user-login-content" onSubmit={handleSubmit(onSubmit)}>
                <h1 className="text-3xl font-bold mb-4">
                    Log In
                </h1>
                <LoginFormField
                    type="email"
                    placeholder="Email"
                    name="email"
                    register={register}
                    error={errors.email}
                />

                <LoginFormField
                    type="password"
                    placeholder="Password"
                    name="password"
                    register={register}
                    error={errors.password}
                />

                <Button className="my-5" variant="outlined" color="primary" type="submit">Login</Button>

                <small className="flex items-baseline">Need an account?
                    <Button href="/register"> Register here</Button>
                </small>
            </form>
        </div>
    );
}