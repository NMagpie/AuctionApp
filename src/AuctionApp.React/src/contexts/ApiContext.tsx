import { createContext, useContext, useEffect, useState } from 'react';
import ApiManager, { baseUrl } from '../api/ApiManager';
import Loading from '../components/Loading/Loading';

const apiManager = new ApiManager(baseUrl);

const ApiContext = createContext<ApiManager>(apiManager);

export const ApiProvider = ({ children }: { children: React.ReactNode | React.ReactNode[] }) => {

    const [didUserLoad, setDidLoad] = useState(false);

    useEffect(() => {
        const getCurrentUser = async () => {
            try {
                await apiManager.getCurrentUser();
            } finally {
                setDidLoad(true);
            }
        };

        getCurrentUser();
    }, [didUserLoad, apiManager.userIdentity]);


    if (!didUserLoad) return <Loading />;

    return (
        <ApiContext.Provider value={apiManager}>
            {children}
        </ApiContext.Provider>
    );
}

export const useApi = () => useContext(ApiContext);