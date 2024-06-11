import { useRouteError } from "react-router-dom";

const ErrorPage = () => {
    const error = useRouteError();

    const errorText = getErrorText(error);

    return <h1 className="text-slate-700 text-center">{errorText}</h1>;
};

const getErrorText = (error) => {

    switch (true) {
        case (error?.response?.status === 404):
            return "Seems like not found...";
        case (error?.response?.status === 401):
            return "I'm not sure you can get here...";
        case (error?.response?.status === 500):
            return "Something went wrong...";
        default:
            return error?.response.message;
    }
};

export default ErrorPage;