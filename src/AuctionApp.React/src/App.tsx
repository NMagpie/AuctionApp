import { useEffect, useState } from 'react'
import './App.css'
import { AuctionReviews } from './apiClient/AuctionReviews';
import { Auctions } from './apiClient/Auctions';
import { Bids } from './apiClient/Bids';
import { Identity } from './apiClient/Identity';
import { Lots } from './apiClient/Lots';
import { Users } from './apiClient/Users';
import { UserWatchlists } from './apiClient/UserWatchlists';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import { AuctionDto } from './apiClient/data-contracts';

function App() {

  const baseUrl : string = "https://localhost:7093";

  const auctionPreviewsApi = new AuctionReviews({
    baseUrl: baseUrl,
  });

  const auctionsApi = new Auctions({
    baseUrl: baseUrl,
  });

  const bidsApi = new Bids({
    baseUrl: baseUrl
  });

  const identityApi = new Identity({
    baseUrl: baseUrl
  });

  const lotsApi = new Lots({
    baseUrl: baseUrl
  });

  const usersApi = new Users({
    baseUrl: baseUrl
  });

  const userWatchlistsApi = new UserWatchlists({
    baseUrl: baseUrl
  });

  const [auction, setAuction] = useState<AuctionDto | null>(null);

  const getAuction = async () => {
    let result = await auctionsApi.auctionsDetail(1);

    setAuction(result.data)
  };

  useEffect(() =>{
    getAuction();
  }, []);

  return (
    <h1 className="text-3xl font-bold underline">
      <p>{JSON.stringify(auction)}</p>
    </h1>
  )
}

export default App
