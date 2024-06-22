import { ApiProvider, useApi } from './contexts/ApiContext';
import { Route, Navigate, createRoutesFromElements, createBrowserRouter, RouterProvider } from "react-router-dom";
import Layout from './components/NavBar/Layout';
import Home from './pages/HomePage/HomePage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegisterPage from './pages/RegisterPage/RegisterPage';
import React from 'react';
import UserPage from './pages/UserPage/UserPage';
import ProductPage from './pages/ProductPage/ProductPage';
import productPageLoader from './pages/ProductPage/ProductPageLoader';
import Loading from './components/Loading/Loading';
import ErrorPage from './pages/ErrorPage/ErrorPage';
import { SnackbarProvider } from 'notistack';
import SearchPage from './pages/SearchPage/SearchPage';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { LocalizationProvider } from '@mui/x-date-pickers';
import searchPageLoader from './pages/SearchPage/SearchPageLoader';
import homePageLoader from './pages/HomePage/HomePageLoader';
import { currentUserPageLoader, userPageLoader } from './pages/UserPage/UserPageLoader';
import ManageProductPage from './pages/ManageProductPage/ManageProductPage';
import editProductLoader from './pages/ManageProductPage/editProductLoader';

import './App.css'

function App() {
  return (
    <SnackbarProvider autoHideDuration={3000} preventDuplicate>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <ApiProvider>
          <AppRouter />
        </ApiProvider>
      </LocalizationProvider>
    </SnackbarProvider>
  )
}

const AppRouter = () => {

  const api = useApi();

  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route
        path="/"
        element={<Layout />}
      >

        <Route
          index
          element={<Home />}
          loader={() => homePageLoader(api)}
          errorElement={<ErrorPage />}
        />
        <Route
          path="*"
          element={<ErrorPage />}
        />

        <Route
          path="login"
          element={<RequireGuest> <LoginPage /> </RequireGuest>}
        />
        <Route
          path="register"
          element={<RequireGuest> <RegisterPage /> </RequireGuest>}
        />

        <Route
          path="search"
          element={<SearchPage />}
          loader={async ({ request }) => searchPageLoader(api, request)}
          errorElement={<ErrorPage />}
        />

        <Route
          path="me"
          element={<RequireAuth> <UserPage /> </RequireAuth>}
          loader={async ({ request }) => currentUserPageLoader(api, request)}
          errorElement={<ErrorPage />}
        />

        <Route
          path="users/:id"
          element={<UserPage />}
          loader={async ({ params, request }) => userPageLoader(api, Number.parseInt(params.id ?? "0"), request)}
          errorElement={<ErrorPage />}
        />

        <Route
          path="products/:id"
          element={<ProductPage />}
          loader={async ({ params }) => productPageLoader(api, params.id)}
          errorElement={<ErrorPage />}
        />

        <Route
          path="create-product"
          element={<RequireAuth> <ManageProductPage /> </RequireAuth>}
        />

        <Route
          path="edit-product/:id"
          element={<RequireAuth> <ManageProductPage /> </RequireAuth>}
          loader={async ({ params }) => editProductLoader(api, params.id)}
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

  const api = useApi();

  return (api.user ? children : <Navigate to="/login" />)
}

function RequireGuest({ children }: { children: React.ReactNode | React.ReactNode[] }) {

  const api = useApi();

  return (!api.user ? children : <Navigate to="/" />)
}

export default App;
