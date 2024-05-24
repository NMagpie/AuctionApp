import './App.css'

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { ApiContext, ApiProvider, User } from './contexts/ApiContext';
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import NoPage from './components/NoPage/NoPage';
import Layout from './components/Layout';
import Home from './components/HomePage/HomePage';
import LoginPage from './components/LoginPage/LoginPage';
import RegisterPage from './components/RegisterPage/RegisterPage';
import React, { useContext, useEffect, useState } from 'react';

function App() {

  return (
    <ApiProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>

            <Route index element={<Home />} />
            <Route path="*" element={<NoPage />} />
            <Route path="/login" element={<RequireGuest> <LoginPage /> </RequireGuest>} />
            <Route path="/register" element={<RequireGuest> <RegisterPage /> </RequireGuest>} />

          </Route>
        </Routes>
      </BrowserRouter>
    </ApiProvider>
  )
}

function RequireAuth(children: React.ReactNode) {

  const apiProvider = useContext(ApiContext);

  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    setUser(apiProvider.user);
  }, [apiProvider.user]);

  return !!user ? children : <Navigate to="/login" />;
}

function RequireGuest(children: React.ReactNode) {

  const apiProvider = useContext(ApiContext);

  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    setUser(apiProvider.user);
  }, [apiProvider.user]);

  return !!user ? children : <Navigate to="/" />;
}

export default App;
