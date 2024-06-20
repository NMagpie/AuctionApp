import ApiManager from "../../api/ApiManager"
import { productDtoToProduct } from "../../common";

const productPageLoader = async (api: ApiManager, id: string) => {

    const intId = Number.parseInt(id);

    const { data: productResult } = await api.products.getProduct({ id: intId });

    const productData = productDtoToProduct(productResult);

    const { data: reviewsData } = await api.productReviews.getPagedReviews({ productId: intId, pageIndex: 0 });

    let watshlistExists = false;

    let canUserLeaveReview = false;

    if (api.userIdentity) {
        try {
            await api.userWatchlists.existsUserWatchlistByProductId({ productId: intId });

            watshlistExists = true;
        } catch (e) {
            if (e.response.status !== 404) {
                throw e;
            }
        }

        canUserLeaveReview = (await api.productReviews.canUserLeaveReview({ id: intId, })).data;
    }

    return { productData, watshlistExists, reviewsData, canUserLeaveReview };
};

export default productPageLoader;