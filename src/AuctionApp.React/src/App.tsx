import './App.css'

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { ApiContext, ApiProvider } from './contexts/ApiContext';
import { useContext, useEffect, useState } from 'react';
import { AuctionDto } from './api';

function App() {

  return (
    <ApiProvider>
      <AuctionInfo />
    </ApiProvider>
  )
}

function AuctionInfo() {

  const [auction, setAuction] = useState<AuctionDto | null>(null);

  const apiProvider = useContext(ApiContext);

  const getAuction = async () => {
    let result = await apiProvider.auctions.auctionsIdGet({ id: 1 });

    setAuction(result.data)
  };

  useEffect(() => {
    getAuction();
  }, []);

  return (
    <h1 className="text-3xl font-bold underline">
      <p>{JSON.stringify(auction)}</p>
    </h1>
  )
}

export default App
