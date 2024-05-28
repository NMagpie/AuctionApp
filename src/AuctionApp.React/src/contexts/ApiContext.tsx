import { createContext, useContext, useEffect, useState } from 'react';
import ApiManager, { baseUrl } from '../api/ApiManager';

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