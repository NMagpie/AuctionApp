import ApiManager from "../../api/ApiManager";

const userPageLoader = async (api: ApiManager, id: string) => {

    const intId = Number.parseInt(id);

    const { data: user } = await api.users.getUser({ id: intId });

    const userProductsQuery = {
        creatorId: intId,
        columnNameForSorting: "StartTime",
        pageSize: 9,
    };

    const { data: userProductsData } = await api.products.searchProducts(userProductsQuery);

    return { user, userProductsData };
};

export default userPageLoader;