import { useLoaderData, useNavigate } from "react-router-dom";
import { Avatar, Button, Pagination, TextField, Typography } from "@mui/material";
import Divider from '@mui/material/Divider';
import { useApi } from "../../contexts/ApiContext";
import { useState } from "react";
import EditIcon from '@mui/icons-material/Edit';
import UserProductList from "../../components/UserProducts/UserProductList";
import { useSnackbar } from "notistack";

import './UserPage.css';

export default function UserPage() {

    const { user, userProductsData } = useLoaderData();

    const api = useApi();

    const navigate = useNavigate();

    const { enqueueSnackbar } = useSnackbar();

    const navigateToPage = (_, value: number) => {
        navigate(`/users/${user.id}?pageIndex=${value - 1}`, { replace: true });

        window.scrollTo(0, 0);
    };

    const isEditable = user.id === api.user?.id;

    const [editMode, setEditMode] = useState(false);

    const [username, setUsername] = useState(user.userName);

    const [editUsername, setEditUsername] = useState(user.userName);

    const changeUsername = (e) => {
        setEditUsername(e.target.value);
    }

    const toggleEditMode = async () => {
        if (editMode) {
            setEditUsername(username);

            if (username !== editUsername) {
                try {
                    const { data } = await api.currentUser.updateUser({
                        updateUserRequest: {
                            email: api.user?.email,
                            userName: editUsername,
                        }
                    });

                    await api.getCurrentUser();

                    setUsername(data.userName);

                    setEditUsername(data.userName);
                } catch (e) {
                    enqueueSnackbar(e, {
                        variant: "error"
                    });
                }
            }
        }
        setEditMode(!editMode);
    };

    return (
        <div className="user-page-info">

            <div className="user-page-info-content">

                {isEditable &&
                    <Button
                        className={`${editMode && "xl:top-[96px] xl:sticky"} edit-button`}
                        onClick={toggleEditMode}
                    >
                        <EditIcon />
                        {editMode ? "Done" : "Edit Profile"}
                    </Button>
                }

                <div className="flex flex-row items-center">
                    <Avatar sx={{ width: 50, height: 50, fontSize: 35 }} className="mr-5" alt={username ?? ""} src="./src" />
                    {editMode ?
                        <>
                            <TextField
                                placeholder="Username"
                                value={editUsername}
                                onChange={changeUsername}
                            />
                        </>
                        :
                        <>
                            <Typography className="font-bold" variant="h5">{username}</Typography>
                        </>
                    }
                </div>

                <Divider className="bg-black border-slate-700 border-solid w-1/2" />

                <Typography className="font-bold" variant="h5">Products:</Typography>

                <UserProductList products={userProductsData.items} editMode={editMode} />

                <Pagination
                    className='mt-5 lg:mt-auto'
                    color="primary"
                    variant="outlined"
                    count={Math.ceil(userProductsData.total / userProductsData.pageSize)}
                    page={Number(userProductsData.pageIndex) + 1}
                    siblingCount={2}
                    onChange={navigateToPage}
                    showFirstButton
                    showLastButton
                    size='large'
                />
            </div>

        </div>
    )
}