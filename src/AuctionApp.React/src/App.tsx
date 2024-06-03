import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { ApiProvider, useApi } from './contexts/ApiContext';
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import NoPage from './components/NoPage/NoPage';
import Layout from './components/NavBar/Layout';
import Home from './components/HomePage/HomePage';
import LoginPage from './components/LoginPage/LoginPage';
import RegisterPage from './components/RegisterPage/RegisterPage';
import React, { useEffect, useState } from 'react';
import UserPage from './components/UserPage/UserPage';
import { User } from './api/ApiManager';
import ProductPage from './components/ProductPage/ProductPage';

import './App.css'

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

            <Route path="/users/:id" element={<UserPage />} />

            <Route path="/products/:id" element={<ProductPage/>}/>

          </Route>
        </Routes>
      </BrowserRouter>
    </ApiProvider>
  )
}

function RequireAuth({ children }: { children: React.ReactNode | React.ReactNode[] }) {

  const api = useApi().api;

  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    setUser(api.user);
  }, [api.user]);

  return user ? children : <Navigate to="/login" />;
}

function RequireGuest({ children }: { children: React.ReactNode | React.ReactNode[] }) {

  const api = useApi().api;

  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    setUser(api.user);
  }, [api.user]);

  return !user ? children : <Navigate to="/" />;
}

export default App;
