import ApiManager from "../../api/ApiManager";

type TabType = "products" | "watchlist" | "participated";

const userPageLoader = async (api: ApiManager, id: number, request: Request) => {

    const url = new URL(request.url);

    const pageIndex = Number.parseInt(url.searchParams.get("pageIndex") ?? "0");

    const tabParam = url.searchParams.get("tab") ?? "products";

    let tab: TabType = tabParam !== "products" &&
        tabParam !== "watchlist" &&
        tabParam !== "participated" ?
        "products" : tabParam;

    if (id !== api.user?.id && tab === "watchlist") {
        tab = "products";
    }

    const { data: user } = await api.users.getUser({ id: id });

    if (tab === "products") {
        const userProductsQuery = {
            creatorId: id,
            columnNameForSorting: "StartTime",
            sortDirection: "desc",
            pageIndex: pageIndex,
            pageSize: 12,
        };

        const { data: userProductsData } = await api.products.searchProducts(userProductsQuery);

        return { user, userProductsData, tab };
    }

    if (tab === "watchlist") {
        const userWatchlistQuery = {
            pageIndex: pageIndex,
            pageSize: 12,
        };

        const { data: userProductsData } = await api.currentUser.getUserWatchlist(userWatchlistQuery);

        return { user, userProductsData, tab };
    }

    if (tab === "participated") {
        const userParticipatedQuery = {
            id: id,
            pageIndex: pageIndex,
            pageSize: 12,
        };

        const { data: userProductsData } = await api.users.getUserParticipatedProducts(userParticipatedQuery);

        return { user, userProductsData, tab };
    }

};

const currentUserPageLoader = async (api: ApiManager, request: Request) => {

    const { user, userProductsData, tab } = await userPageLoader(api, api.user?.id, request);

    return { user, userProductsData, tab };
};

export { userPageLoader, currentUserPageLoader };
export type { TabType };