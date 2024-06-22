import axios, { AxiosInstance } from "axios";
import { Configuration, BidsApi, IdentityApi, UsersApi, CurrentUserApi, UserWatchlistsApi, AccessTokenResponse, CurrentUserDto, ProductReviewsApi, ProductsApi, SearchRecordsApi } from "../api/openapi-generated";

export const baseUrl: string = process.env.REACT_APP_BASE_URL;

class UserIdentity {

    accessToken: string | null | undefined;

    refreshToken: string | null | undefined;

    expireDate: Date | null | undefined;

    public isViable = () => this.expireDate ? this.expireDate >= new Date() : false
}

export class User {
    id: number | null | undefined;

    userName: string | null | undefined;

    email: string | null | undefined;

    balance: number | null | undefined;

    reservedBalance: number | null | undefined;
}

export default class ApiManager {

    productReviews: ProductReviewsApi;

    products: ProductsApi;

    bids: BidsApi;

    identity: IdentityApi;

    users: UsersApi;

    currentUser: CurrentUserApi;

    userWatchlists: UserWatchlistsApi;

    userIdentity: UserIdentity | null;

    searchRecords: SearchRecordsApi;

    user: User | null;

    constructor(baseUrl: string) {

        const configuration = new Configuration({
            basePath: baseUrl,
            accessToken: () => this.retrieveAccessToken()
        });

        this.productReviews = new ProductReviewsApi(configuration);

        this.products = new ProductsApi(configuration);

        this.bids = new BidsApi(configuration);

        this.identity = new IdentityApi(configuration);

        this.users = new UsersApi(configuration);

        this.currentUser = new CurrentUserApi(configuration);

        this.userWatchlists = new UserWatchlistsApi(configuration);

        this.searchRecords = new SearchRecordsApi(configuration);

        const userIdentityString = localStorage.getItem('userIdentity');

        this.userIdentity = null;

        if (userIdentityString) {
            const parsedUserIdentity = JSON.parse(userIdentityString);

            parsedUserIdentity.expireDate = new Date(parsedUserIdentity.expireDate);

            this.userIdentity = parsedUserIdentity;
        }

        this.user = null;
    }

    public retrieveAccessToken = async () => {

        if (!this.userIdentity) {
            return null;
        }

        if (this.userIdentity.expireDate <= new Date()) {
            await this.refreshAccessToken();
        }

        return this.userIdentity.accessToken;
    }

    public login = async (email: string, password: string) => {

        const requestTime = Date.now();

        const { data } = await this.identity.identityLoginPost({ loginRequest: { email, password } });

        this.assignUserIdentity(data, requestTime);

        const user = (await this.currentUser.getCurrentUser()).data;

        this.assignUser(user);
    }

    public register = async (email: string, password: string) => {

        await this.identity.identityRegisterPost({ registerRequest: { email, password } });

        await this.login(email, password);
    }

    public logout = () => {
        this.assignUserIdentity(null);

        this.assignUser(null);
    }

    public getCurrentUser = async () => {

        const userValue = this.userIdentity ? (await this.currentUser.getCurrentUser()).data : null;

        this.assignUser(userValue);

    }

    private refreshAccessToken = async () => {

        const requestTime = Date.now();

        const { data } = await this.identity.identityRefreshPost({ refreshRequest: { refreshToken: this.userIdentity!.refreshToken } });

        this.assignUserIdentity(data, requestTime);

        return data.accessToken;
    }

    private assignUserIdentity = (data: AccessTokenResponse | null, requestTime: number | null = null) => {
        if (!data) {
            this.userIdentity = null;

            localStorage.removeItem('userIdentity');

            return;
        }

        const updatedIdentity = {
            accessToken: data.accessToken!,
            refreshToken: data.refreshToken!,
            expireDate: new Date((requestTime ?? Date.now()) + data.expiresIn! * 1000),
        };

        this.userIdentity = updatedIdentity;

        localStorage.setItem('userIdentity', JSON.stringify(this.userIdentity));
    }

    private assignUser = (data: CurrentUserDto | null) => {
        if (!data) {
            this.user = null;

            return;
        }

        const updatedUser = {
            id: data.id!,
            userName: data.userName!,
            balance: data.balance!,
            email: data.email!,
            reservedBalance: data.reservedBalance!,
        };

        this.user = updatedUser;
    }
}