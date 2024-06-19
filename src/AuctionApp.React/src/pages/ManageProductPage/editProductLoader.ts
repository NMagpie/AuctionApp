import ApiManager from "../../api/ApiManager";

const editProductLoader = async (api: ApiManager, id: string) => {

    const intId = Number.parseInt(id ?? "0");

    const { data: productData } = await api.products.getProduct({ id: intId });

    return { productData };
};

export default editProductLoader;