import axios, { AxiosInstance } from "axios";
import { Configuration, BidsApi, IdentityApi, UsersApi, CurrentUserApi, UserWatchlistsApi, AccessTokenResponse, CurrentUserDto, ProductReviewsApi, ProductsApi } from "../api/openapi-generated";

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

    balance: number | null | undefined;
}

export default class ApiManager {

    productReviews: ProductReviewsApi;

    products: ProductsApi;

    bids: BidsApi;

    identity: IdentityApi;

    users: UsersApi;

    currentUser: CurrentUserApi;

    userWatchlsits: UserWatchlistsApi;

    userIdentity: UserIdentity | null;

    user: User | null;

    axios: AxiosInstance;

    constructor(baseUrl: string) {

        this.axios = axios.create();

        // this.axios.interceptors.request.use(async (config) => {

        //     const token = await this.retrieveAccessToken();

        //     if (token) {
        //         config.headers["Authorization"] = `Bearer ${token}`;
        //     }

        //     return config;
        // });

        const configuration = new Configuration({
            basePath: baseUrl,
            accessToken: () => this.retrieveAccessToken()
        });

        this.productReviews = new ProductReviewsApi(configuration, undefined, this.axios);

        this.products = new ProductsApi(configuration, undefined, this.axios);

        this.bids = new BidsApi(configuration, undefined, this.axios);

        this.identity = new IdentityApi(configuration, undefined, this.axios);

        this.users = new UsersApi(configuration, undefined, this.axios);

        this.currentUser = new CurrentUserApi(configuration, undefined, this.axios);

        this.userWatchlsits = new UserWatchlistsApi(configuration, undefined, this.axios);

        const userIdentityString = localStorage.getItem('userIdentity');

        this.userIdentity = userIdentityString ? JSON.parse(userIdentityString) : null;

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

        this.userIdentity = new UserIdentity();
        this.userIdentity.accessToken = data.accessToken!;
        this.userIdentity.refreshToken = data.refreshToken!;
        this.userIdentity.expireDate = new Date((requestTime ?? Date.now()) + data.expiresIn! * 1000);

        localStorage.setItem('userIdentity', JSON.stringify(this.userIdentity));
    }

    private assignUser = (data: CurrentUserDto | null) => {
        if (!data) {
            this.user = null;

            return;
        }

        this.user = new User();
        this.user.id = data.id!;
        this.user.userName = data.userName!;
        this.user.balance = data.balance!;
    }
}