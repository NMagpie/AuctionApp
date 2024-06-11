import ApiManager from "../../api/ApiManager"

const productLoader = async (api: ApiManager, id: string) => {

    const intId = Number.parseInt(id);

    const { data: productResult } = await api.products.getProduct({ id: intId });

    const productData = {
        id: productResult.id ?? 0,
        title: productResult.title ?? '',
        description: productResult.description ?? '',
        creator: productResult.creator ?? null,
        startTime: productResult.startTime ? new Date(productResult.startTime) : null,
        endTime: productResult.endTime ? new Date(productResult.endTime) : null,
        bids: productResult.bids ?? [],
    };

    const { data: reviewsData } = await api.productReviews.getPagedReviews({ productId: intId, pageIndex: 0 });

    // const date = new Date();

    // date.setHours(date.getHours() + 1);

    // productData.endTime = date;

    // const date1 = new Date();

    // date1.setSeconds(date1.getSeconds());

    // productData.startTime = date1;

    let watshlistExists = false;

    if (api.userIdentity) {
        try {
            await api.userWatchlsits.existsUserWatchlistByProductId({ productId: intId })

            watshlistExists = true;
        } catch (e) {
            if (e.response.status !== 404) {
                throw e;
            }
        }
    }

    return { productData, watshlistExists, reviewsData };
};

export default productLoader;