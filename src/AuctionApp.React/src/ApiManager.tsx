import { AuctionReviews } from './apiClient/AuctionReviews';
import { Auctions } from './apiClient/Auctions';
import { HttpClient } from './apiClient/http-client';

export default class ApiManager {
    private static apis: Record<T, any> = {};

    static {
        let baseUrl: string = process.env.API_BASE_URL;

        const apiConfigs = {
            auctionReviewsApi: AuctionReviews,
            auctionsApi: Auctions,
        };

        for (const [apiName, ApiClass] of Object.entries(apiConfigs)) {
            ApiManager.apis[apiName] = new ApiClass({ baseUrl });
        }
    }

    static getApi<T extends HttpClient>(): any {
        return ApiManager.apis[apiName];
    }
}