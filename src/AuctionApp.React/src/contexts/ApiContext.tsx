import { createContext, useContext, useEffect, useState } from 'react';
import { AccessTokenResponse, AuctionReviewsApi, AuctionsApi, BidsApi, Configuration, CurrentUserApi, CurrentUserDto, IdentityApi, LotsApi, UserDto, UserWatchlistsApi, UsersApi } from '../api';
import axios, { AxiosInstance } from 'axios';

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

        this.currentUser = new CurrentUserApi(configuration, undefined, this.axios);

        this.userWatchlsits = new UserWatchlistsApi(configuration, undefined, this.axios);

        const userIdentityString = localStorage.getItem('userIdentity');

        this.userIdentity = userIdentityString ? JSON.parse(userIdentityString) : null;

        this.user = null;

        if (this?.userIdentity?.accessToken) {
            this.axios.defaults.headers.common['Authorization'] = `Bearer ${this.userIdentity.accessToken}`;
        }
    }

    auctionReviews: AuctionReviewsApi;

    auctions: AuctionsApi;

    bids: BidsApi;

    identity: IdentityApi;

    lots: LotsApi;

    users: UsersApi;

    currentUser: CurrentUserApi;

    userWatchlsits: UserWatchlistsApi;

    userIdentity: UserIdentity | null;

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

        const user = (await this.currentUser.meGet()).data;

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

        const userValue = this.userIdentity ? (await this.currentUser.meGet()).data : null;

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

const baseUrl: string = process.env.REACT_APP_BASE_URL;

const apiManager = new ApiManager(baseUrl);

interface ApiContextType {
    api: ApiManager;
    didUserLoad: boolean;
}

const ApiContext = createContext<ApiContextType>({ api: apiManager, didUserLoad: false });

export const ApiProvider = ({ children }: { children: React.ReactNode | React.ReactNode[] }) => {

    const [didUserLoad, setDidLoad] = useState(false);

    useEffect(() => {
        const getCurrentUser = async () => {
            await apiManager.getCurrentUser();

            setDidLoad(true);
        };

        getCurrentUser();
    }, [didUserLoad, apiManager.userIdentity]);

    return (
        <ApiContext.Provider value={{ api: apiManager, didUserLoad }}>
            {children}
        </ApiContext.Provider>
    );
}

export const useApi = () => useContext(ApiContext);