import { createContext, ReactNode } from 'react';
import { AccessTokenResponse, AuctionReviewsApi, AuctionsApi, BidsApi, Configuration, IdentityApi, LotsApi, UserWatchlistsApi, UsersApi } from '../api';
import axios, { AxiosInstance } from 'axios';

class UserManager {

    constructor() {
        this.accessToken = localStorage.getItem('accessToken');
        this.refreshToken = localStorage.getItem('refreshToken');
        const expireDate = localStorage.getItem('expireDate');

        this.expireDate = expireDate ? new Date(expireDate) : null;
    }

    accessToken: string | null;

    refreshToken: string | null;

    expireDate: Date | null;

    public isExpired = () => {
        return this.expireDate ? this.expireDate <= new Date() : true;
    }
}

class ApiManager {

    constructor(baseUrl: string) {

        this.axios = axios.create();

        const configuration = new Configuration({
            basePath: baseUrl,
        });

        this.axios.interceptors.response.use(
            (response) => response,
            async (error) => {
                if (error.response && error.response.status === 401 && this?.user?.refreshToken) {
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

        this.auctionReviews = new AuctionReviewsApi(configuration, undefined, this.axios);

        this.auctions = new AuctionsApi(configuration, undefined, this.axios);

        this.bids = new BidsApi(configuration, undefined, this.axios);

        this.identity = new IdentityApi(configuration, undefined, this.axios);

        this.lots = new LotsApi(configuration, undefined, this.axios);

        this.users = new UsersApi(configuration, undefined, this.axios);

        this.userWatchlsits = new UserWatchlistsApi(configuration, undefined, this.axios);

        this.user = new UserManager();

        if (this.user.accessToken && !this.user.isExpired()) {
            this.axios.defaults.headers.common['Authorization'] = `Bearer ${this.user.accessToken}`;
        }
    }

    auctionReviews: AuctionReviewsApi;

    auctions: AuctionsApi;

    bids: BidsApi;

    identity: IdentityApi;

    lots: LotsApi;

    users: UsersApi;

    userWatchlsits: UserWatchlistsApi;

    user: UserManager | null;

    private axios: AxiosInstance;

    private refreshAccessToken = async () => {
        const { data } = await this.identity.identityRefreshPost({ refreshRequest: { refreshToken: this.user!.refreshToken } });

        this.assignUser(data);

        return data.accessToken;
    }

    public login = async (email: string, password: string) => {
        const { data } = await this.identity.identityLoginPost({ loginRequest: { email, password } });

        this.assignUser(data);

        this.axios.defaults.headers.common['Authorization'] = `Bearer ${data.accessToken}`;
    }

    public logout = () => {
        this.assignUser(null);

        this.axios.defaults.headers.common['Authorization'] = "";
    }

    private assignUser = (data: AccessTokenResponse | null) => {
        if (!data) {
            this.user = null;

            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('expireDate');

            return;
        }

        const {
            accessToken = null,
            refreshToken = null,
            expiresIn = null,
        } = data

        this.user = new UserManager();
        this.user.accessToken = accessToken;
        this.user.refreshToken = refreshToken;
        this.user.expireDate = expiresIn ? new Date(Date.now() + expiresIn * 1000) : null;

        localStorage.setItem('accessToken', accessToken!);
        localStorage.setItem('refreshToken', refreshToken!);
        localStorage.setItem('expireDate', this.user.expireDate!.toString());
    }
}

export interface Props {
    children: ReactNode | ReactNode[];
}

const baseUrl: string = process.env.REACT_APP_BASE_URL;

const apiManager = new ApiManager(baseUrl);

const ApiContext = createContext(apiManager);

const ApiProvider = ({ children }: Props) => {
    return (
        <ApiContext.Provider value={apiManager}>
            {children}
        </ApiContext.Provider>
    );
}

export { ApiContext, ApiProvider };