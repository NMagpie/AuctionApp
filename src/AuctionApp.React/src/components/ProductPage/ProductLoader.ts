import ApiManager from "../../api/ApiManager"

const productLoader = async (api: ApiManager, id: string) => {

    const intId = Number.parseInt(id);

    let productData = null;

    const { data } = (await api.products.getProduct({ id: intId }));

    productData = {
        id: data.id ?? 0,
        title: data.title ?? '',
        description: data.description ?? '',
        creator: data.creator ?? null,
        startTime: data.startTime ? new Date(data.startTime) : null,
        endTime: data.endTime ? new Date(data.endTime) : null,
        bids: data.bids ?? [],
    };

    let watchlistData = null;

    if (api.userIdentity) {
        try {
            watchlistData = (await api.userWatchlsits.getUserWatchlistByProductId({ productId: intId })).data
        } catch (e) {
            console.log(e);

        }
    }

    return { productData, watchlistData };
};

export default productLoader;