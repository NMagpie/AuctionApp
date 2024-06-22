import ApiManager from "../../api/ApiManager"

const homePageLoader = async (api: ApiManager) => {
    const { items: upcomingProducts } = (await api.products.searchProducts({ searchPreset: "ComingSoon", pageSize: 3})).data;

    const { items: mostActiveProducts } = (await api.products.searchProducts({ searchPreset: "MostActive", pageSize: 3})).data;

    return { upcomingProducts, mostActiveProducts };
}

export default homePageLoader;