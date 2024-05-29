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

    constructor(baseUrl: string) {

        this.axios = axios.create();

        const configuration = new Configuration({
            basePath: baseUrl,
        });

        this.axios.interceptors.response.use(
            (response) => response,
            async (error) => {
                if (error.response && error.response.status === 401 && this?.userIdentity?.refreshToken) {
                    try {
                        const newAccessToken = await this.refreshAccessToken();
                        error.config.headers['Authorization'] = `Bearer ${newAccessToken}`;
                        return this.axios(error.config);
                    } catch (refreshError) {
                        throw refreshError;
                    }
                }
                return Promise.reject(error);
            }
        );

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

        if (this?.userIdentity?.accessToken) {
            this.axios.defaults.headers.common['Authorization'] = `Bearer ${this.userIdentity.accessToken}`;
        }
    }

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

    public refreshAccessToken = async () => {
        const { data } = await this.identity.identityRefreshPost({ refreshRequest: { refreshToken: this.userIdentity!.refreshToken } });

        this.assignUserIdentity(data);

        return data.accessToken;
    }

    public login = async (email: string, password: string) => {

        const { data } = await this.identity.identityLoginPost({ loginRequest: { email, password } });

        this.assignUserIdentity(data);

        this.axios.defaults.headers.common['Authorization'] = `Bearer ${data.accessToken}`;

        const user = (await this.currentUser.usersMeGet()).data;

        this.assignUser(user);
    }

    public register = async (email: string, password: string) => {

        await this.identity.identityRegisterPost({ registerRequest: { email, password } });

        await this.login(email, password);
    }

    public logout = () => {
        this.assignUserIdentity(null);

        this.assignUser(null);

        this.axios.defaults.headers.common['Authorization'] = "";
    }

    public getCurrentUser = async () => {

        const userValue = this.userIdentity ? (await this.currentUser.usersMeGet()).data : null;

        this.assignUser(userValue);

    }

    private assignUserIdentity = (data: AccessTokenResponse | null) => {
        if (!data) {
            this.userIdentity = null;

            localStorage.removeItem('userIdentity');

            return;
        }

        this.userIdentity = new UserIdentity();
        this.userIdentity.accessToken = data.accessToken!;
        this.userIdentity.refreshToken = data.refreshToken!;
        this.userIdentity.expireDate = new Date(Date.now() + data.expiresIn! * 1000);

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