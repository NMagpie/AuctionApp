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
import productPageLoader from './pages/ProductPage/ProductPageLoader';
import Loading from './components/Loading/Loading';
import ErrorPage from './pages/ErrorPage/ErrorPage';
import { SnackbarProvider } from 'notistack';
import SearchPage from './pages/SearchPage/SearchPage';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import CreateProductPage from './pages/CreateProductPage/CreateProductPage';
import { LocalizationProvider } from '@mui/x-date-pickers';
import searchPageLoader from './pages/SearchPage/SearchPageLoader';

import './App.css'
import homePageLoader from './pages/HomePage/HomePageLoader';
import userPageLoader from './pages/UserPage/UserPageLoader';

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

  const { api } = useApi();

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
          path="/login"
          element={<RequireGuest> <LoginPage /> </RequireGuest>}
        />
        <Route
          path="/register"
          element={<RequireGuest> <RegisterPage /> </RequireGuest>}
        />

        <Route path="/search"
          element={<SearchPage />}
          loader={async ({ request }) => searchPageLoader(api, request)}
          errorElement={<ErrorPage />}
        />

        <Route path="/users/:id"
          element={<UserPage />}
          loader={async ({ params }) => userPageLoader(api, params.id)}
          errorElement={<ErrorPage />}
        />

        <Route path="/products/:id"
          element={<ProductPage />}
          loader={async ({ params }) => productPageLoader(api, params.id)}
          errorElement={<ErrorPage />}
        />

        <Route path="/create-product"
          element={<RequireAuth> <CreateProductPage /> </RequireAuth>}
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

  const { didUserLoad, user } = useApi();

  return (
    <>
      {didUserLoad ?
        user ? children : <Navigate to="/login" /> :
        <Loading />
      }
    </>
  )
}

function RequireGuest({ children }: { children: React.ReactNode | React.ReactNode[] }) {

  const { didUserLoad, user } = useApi();

  return (
    <>
      {didUserLoad ?
        !user ? children : <Navigate to="/" /> :
        <Loading />
      }
    </>
  )
}

export default App;
