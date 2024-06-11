import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useApi } from "../../contexts/ApiContext";
import { Avatar, Typography } from "@mui/material";
import Divider from '@mui/material/Divider';
import { UserDto } from "../../api/openapi-generated";

import './UserPage.css';

export default function UserPage() {

    const { id } = useParams();

    const [user, setUser] = useState<UserDto | null>(null);

    const api = useApi().api;

    const getUser = async () => {
        const user = (await api.users.getUser({ id: parseInt(id ?? "") })).data;

        setUser(user);
    };

    console.log();

    useEffect(() => {
        getUser();
    }, []);

    return (
        <div className="user-page-info">

            <div className="user-page-info-content">
                <div className="flex flex-row items-center">
                    <Avatar sx={{ width: 50, height: 50, fontSize: 35 }} className="mr-5" alt={user?.userName ?? ""} src="./src" />
                    <Typography variant="h5">{user?.userName}</Typography>
                </div>

                <Divider className="bg-black border-slate-700 border-solid w-1/2" />

                <Typography variant="h5">Auctions:</Typography>
            </div>

        </div>
    )
}