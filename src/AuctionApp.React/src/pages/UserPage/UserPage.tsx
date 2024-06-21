import { useLoaderData, useNavigate } from "react-router-dom";
import { Avatar, Button, Pagination, Tab, Tabs, TextField, Typography } from "@mui/material";
import Divider from '@mui/material/Divider';
import { useApi } from "../../contexts/ApiContext";
import { useState } from "react";
import EditIcon from '@mui/icons-material/Edit';
import UserProductList from "../../components/UserPageSections/UserProductList";
import { useSnackbar } from "notistack";
import UserWatchlistList from "../../components/UserPageSections/UserWatchlistList";
import UserParticipatedList from "../../components/UserPageSections/UserParticipatedList";

import ShoppingBagIcon from '@mui/icons-material/ShoppingBag';
import GavelIcon from '@mui/icons-material/Gavel';
import BookmarkIcon from '@mui/icons-material/Bookmark';

import './UserPage.css';

export default function UserPage() {

    const { user, userProductsData, tab } = useLoaderData();

    const api = useApi();

    const navigate = useNavigate();

    const { enqueueSnackbar } = useSnackbar();

    const navigateToPage = (_, value: number) => {
        navigate(`/${isEditable ? "me" : `users/${user.id}`}?tab=${tab}&pageIndex=${value - 1}`, { replace: true });

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

    const tabStyle = (tabValue: string) => tab === tabValue ? "text-slate-900" : "text-slate-500";

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

                <Tabs
                    TabIndicatorProps={{ style: { display: "none" } }}
                    className="bg-slate-300 px-5 py-1"
                    value={tab}
                    onChange={(_, value) => { navigate(`/${isEditable ? "me" : `users/${user.id}`}?tab=${value}`); }}
                >

                    <Tab
                        icon={<ShoppingBagIcon />}
                        className={`w-1/3 overflow-visible ${tabStyle("products")}`}
                        value="products"
                        label="Products"
                    />
                    {isEditable &&
                        <Tab
                            icon={<BookmarkIcon />}
                            className={`w-1/3 overflow-visible ${tabStyle("watchlist")}`}
                            value="watchlist"
                            label="Watchlist"
                        />}
                    <Tab
                        icon={<GavelIcon />}
                        className={`w-1/3 overflow-visible ${tabStyle("participated")}`}
                        value="participated"
                        label="Participated"
                    />

                </Tabs>

                {tab === "products" && <UserProductList products={userProductsData.items} editMode={editMode} />}

                {tab === "watchlist" && <UserWatchlistList products={userProductsData.items} />}

                {tab === "participated" && <UserParticipatedList products={userProductsData.items} userId={user.id} />}

                {userProductsData.items.length !== 0 && <Pagination
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
                />}
            </div>

        </div>
    )
}