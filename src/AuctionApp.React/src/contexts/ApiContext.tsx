import { createContext, ReactNode } from 'react';
import { AccessTokenResponse, AuctionReviewsApi, AuctionsApi, BidsApi, Configuration, IdentityApi, LotsApi, UserDto, UserWatchlistsApi, UsersApi } from '../api';
import axios, { AxiosInstance } from 'axios';

class UserIdentity {

    constructor() {
        this.accessToken = localStorage.getItem('accessToken');
        this.refreshToken = localStorage.getItem('refreshToken');
        const expireDate = localStorage.getItem('expireDate');

        this.expireDate = expireDate ? new Date(expireDate) : null;
    }

    accessToken: string | null;

    refreshToken: string | null;

    expireDate: Date | null;

    public isViable = () => {
        return this.expireDate ? this.expireDate >= new Date() : false;
    }
}

export class User {

    constructor() {
        this.id = Number.parseInt(localStorage.getItem('userId') ?? "");
        this.userName = localStorage.getItem('userName');
        this.balance = Number.parseFloat(localStorage.getItem('userBalance') ?? "");
    }

    id: number | null;

    userName: string | null;

    balance: number | null;
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

        this.auctionReviews = new AuctionReviewsApi(configuration, undefined, this.axios);

        this.auctions = new AuctionsApi(configuration, undefined, this.axios);

        this.bids = new BidsApi(configuration, undefined, this.axios);

        this.identity = new IdentityApi(configuration, undefined, this.axios);

        this.lots = new LotsApi(configuration, undefined, this.axios);

        this.users = new UsersApi(configuration, undefined, this.axios);

        this.userWatchlsits = new UserWatchlistsApi(configuration, undefined, this.axios);

        if (localStorage.getItem('refreshToken')) {
            this.userIdentity = new UserIdentity();
            this.user = new User();
        } else {
            this.userIdentity = null;
            this.user = null;
        }

        if (this?.userIdentity?.accessToken && this?.userIdentity.isViable()) {
            this.axios.defaults.headers.common['Authorization'] = `Bearer ${this.userIdentity.accessToken}`;
        }
    }

    auctionReviews: AuctionReviewsApi;

    auctions: AuctionsApi;

    bids: BidsApi;

    identity: IdentityApi;

    lots: LotsApi;

    users: UsersApi;

    userWatchlsits: UserWatchlistsApi;

    private userIdentity: UserIdentity | null;

    user: User | null;

    private axios: AxiosInstance;

    private refreshAccessToken = async () => {
        const { data } = await this.identity.identityRefreshPost({ refreshRequest: { refreshToken: this.userIdentity!.refreshToken } });

        this.assignUserIdentity(data);

        return data.accessToken;
    }

    public login = async (email: string, password: string) => {

        const { data } = await this.identity.identityLoginPost({ loginRequest: { email, password } });

        this.assignUserIdentity(data);

        this.axios.defaults.headers.common['Authorization'] = `Bearer ${data.accessToken}`;

        const user = (await this.users.usersCurrentUserGet()).data;

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

    private assignUserIdentity = (data: AccessTokenResponse | null) => {
        if (!data) {
            this.userIdentity = null;

            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('expireDate');

            return;
        }

        this.userIdentity = new UserIdentity();
        this.userIdentity.accessToken = data.accessToken!;
        this.userIdentity.refreshToken = data.refreshToken!;
        this.userIdentity.expireDate = new Date(Date.now() + data.expiresIn! * 1000);

        localStorage.setItem('accessToken', data.accessToken!);
        localStorage.setItem('refreshToken', data.refreshToken!);
        localStorage.setItem('expireDate', this.userIdentity.expireDate!.toString());
    }

    private assignUser = (data: UserDto | null) => {
        if (!data) {
            this.user = null;

            localStorage.removeItem('userId');
            localStorage.removeItem('userName');
            localStorage.removeItem('userBalance');

            return;
        }

        this.user = new User();

        this.user.id = data.id!;
        this.user.userName = data.userName!;
        this.user.balance = data.balance!;

        localStorage.setItem('userId', data.id!.toString());
        localStorage.setItem('userName', data.userName!);
        localStorage.setItem('userBalance', data.balance!.toString());
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