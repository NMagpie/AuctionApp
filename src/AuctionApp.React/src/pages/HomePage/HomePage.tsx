import { useApi } from "../../contexts/ApiContext";

const HomePage = () => {

    const { api } = useApi();

    return (
        <div>
            <h1 className="text-slate-700">Hello{api.userIdentity && `, ${api.user?.userName}`}!</h1>
        </div>
    );
};

export default HomePage;