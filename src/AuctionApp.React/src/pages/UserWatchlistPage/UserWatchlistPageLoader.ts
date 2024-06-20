import ApiManager from "../../api/ApiManager";

const userWatchlistPageLoader = async (api: ApiManager, request: Request) => {

    const url = new URL(request.url);

    const pageIndex = Number.parseInt(url.searchParams.get("pageIndex") ?? "0");

    const userWatchlistQuery = {
        pageIndex: pageIndex,
        pageSize: 12,
    };

    const { data: userWatchlistData } = await api.currentUser.getUserWatchlist(userWatchlistQuery);

    return { userWatchlistData };
};

export default userWatchlistPageLoader;