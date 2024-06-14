import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { ApiProvider, useApi } from './contexts/ApiContext';
import { Route, Navigate, createRoutesFromElements, createBrowserRouter, RouterProvider } from "react-router-dom";
import Layout from './components/NavBar/Layout';
import Home from './pages/HomePage/HomePage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegisterPage from './pages/RegisterPage/RegisterPage';
import React from 'react';
import UserPage from './pages/UserPage/UserPage';
import ProductPage from './pages/ProductPage/ProductPage';
import productLoader from './pages/ProductPage/ProductLoader';
import Loading from './components/LoadingComponent/Loading';
import ErrorPage from './pages/ErrorPage/ErrorPage';
import { SnackbarProvider } from 'notistack';
import SearchPage from './pages/SearchPage/SearchPage';
import searchLoader from './pages/SearchPage/SearchLoader';

import './App.css'

function App() {
  return (
    <SnackbarProvider autoHideDuration={3000} preventDuplicate>
      <ApiProvider>
        <AppRouter />
      </ApiProvider>
    </SnackbarProvider>
  )
}

const AppRouter = () => {

  const { api } = useApi();

  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/" element={<Layout />}>

        <Route index element={<Home />} />
        <Route path="*" element={<ErrorPage />} />

        <Route path="/login" element={<RequireGuest> <LoginPage /> </RequireGuest>} />
        <Route path="/register" element={<RequireGuest> <RegisterPage /> </RequireGuest>} />

        <Route path="/search"
          element={<SearchPage />}
          loader={async ({ request }) => searchLoader(api, request)}
          errorElement={<ErrorPage />}
        />

        <Route path="/users/:id" element={<UserPage />} />

        <Route path="/products/:id"
          element={<ProductPage />}
          loader={async ({ params }) => productLoader(api, params.id)}
          errorElement={<ErrorPage />}
        />

      </Route>
    )
  );

  return (
    <RouterProvider
      router={router}
      fallbackElement={<Loading />}
    />
  );
}

function RequireAuth({ children }: { children: React.ReactNode | React.ReactNode[] }) {

  const { user } = useApi().api;

  return user ? children : <Navigate to="/login" />;
}

function RequireGuest({ children }: { children: React.ReactNode | React.ReactNode[] }) {

  const { user } = useApi().api;

  return !user ? children : <Navigate to="/" />;
}

export default App;
