import { createContext, useContext, useEffect, useState } from 'react';
import ApiManager, { User, baseUrl } from '../api/ApiManager';

const apiManager = new ApiManager(baseUrl);

interface ApiContextType {
    api: ApiManager;
    didUserLoad: boolean;
    user: User | null;
}

const ApiContext = createContext<ApiContextType>({ api: apiManager, didUserLoad: false });

export const ApiProvider = ({ children }: { children: React.ReactNode | React.ReactNode[] }) => {

    const [didUserLoad, setDidLoad] = useState(false);

    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        const getCurrentUser = async () => {
            try {
                await apiManager.getCurrentUser();

                setUser(apiManager.user);
            } catch (e) {
                setUser(null);
            } finally {
                setDidLoad(true);
            }
        };

        getCurrentUser();
    }, [didUserLoad, apiManager.userIdentity]);



    return (
        <ApiContext.Provider value={{ api: apiManager, didUserLoad, user }}>
            {children}
        </ApiContext.Provider>
    );
}

export const useApi = () => useContext(ApiContext);