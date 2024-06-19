import ApiManager from "../../api/ApiManager";

const userPageLoader = async (api: ApiManager, id: number, request: Request) => {

    const url = new URL(request.url);

    const pageIndex = Number.parseInt(url.searchParams.get("pageIndex") ?? "0");

    const { data: user } = await api.users.getUser({ id: id });

    const userProductsQuery = {
        creatorId: id,
        columnNameForSorting: "StartTime",
        sortDirection: "desc",
        pageIndex: pageIndex,
        pageSize: 12,
    };

    const { data: userProductsData } = await api.products.searchProducts(userProductsQuery);

    return { user, userProductsData };
};

const currentUserPageLoader = async (api: ApiManager, request: Request) => {

    const { user, userProductsData } = await userPageLoader(api, api.user?.id, request);

    return { user, userProductsData };
};

export { userPageLoader, currentUserPageLoader };