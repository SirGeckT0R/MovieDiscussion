import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App.tsx';
import {
  createBrowserRouter,
  Navigate,
  Outlet,
  RouterProvider,
} from 'react-router-dom';
import { queryClient } from './api/global.ts';
import { QueryClientProvider } from '@tanstack/react-query';
import { ErrorPage } from './pages/ErrorPage/ErrorPage.tsx';
import Index from './pages/Index.tsx';
import { globalLoader } from './loaders/loader.tsx';
import { AuthProvider } from './providers/AuthProvider.tsx';
import { ProtectedRoute } from './components/ProtectedRoute.tsx';
import { Role } from './types/user.ts';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import {
  getListOfMoviesQuery,
  getMovieQuery,
} from './queries/moviesQueries.tsx';
import { CardLoader } from './components/CardLoading.tsx';
import { CreateMoviePage } from './pages/CreateMoviePage/CreateMoviePage.tsx';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { getGenresQuery } from './queries/genresQueries.tsx';
import { Skeleton } from '@mui/material';
import { LoginPage } from './pages/LoginPage/LoginPage.tsx';
import { RegisterPage } from './pages/RegisterPage/RegisterPage.tsx';
import { ListOfMoviesPage } from './pages/ListOfMoviesPage/ListOfMoviesPage.tsx';
import { ModifyGenresPage } from './pages/ModifyGenresPage/ModifyGenresPage.tsx';
import { Header } from './components/Header.tsx';
import { MoviePage } from './pages/MoviePage/MoviePage.tsx';
import { UpdateMoviePage } from './pages/UpdateMoviePage/UpdateMoviePage.tsx';
import { UserProfilePage } from './pages/UserProfile/UserProfilePage.tsx';
import { getUserProfileQuery } from './queries/profilesQueries.tsx';
import { Chat } from './pages/Chat/Chat.tsx';
import { DiscussionPage } from './pages/DiscussionsPage/DiscussionPage.tsx';
import { getDiscussionsQuery } from './queries/discussionsQueries.tsx';

const router = createBrowserRouter([
  {
    path: '/',
    element: (
      <AuthProvider>
        <Header />
        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <App />
        </LocalizationProvider>
      </AuthProvider>
    ),
    children: [
      {
        index: true,
        element: <Index />,
      },
      { path: '/login', element: <LoginPage /> },
      { path: '/register', element: <RegisterPage /> },
      {
        path: '/movies',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: <ListOfMoviesPage />,
            loader: globalLoader(queryClient, getListOfMoviesQuery),
            HydrateFallback: CardLoader,
          },
          {
            path: ':id',
            element: <MoviePage />,
            loader: globalLoader(queryClient, getMovieQuery),
            HydrateFallback: Skeleton,
          },
          {
            path: 'new',
            element: (
              <ProtectedRoute allowedRoles={[Role.User, Role.Admin]}>
                <CreateMoviePage />
              </ProtectedRoute>
            ),
            loader: globalLoader(queryClient, getGenresQuery),
            HydrateFallback: Skeleton,
          },
          {
            path: ':id/edit',
            element: (
              <ProtectedRoute allowedRoles={[Role.Admin]}>
                <UpdateMoviePage />
              </ProtectedRoute>
            ),
            loader: globalLoader(queryClient, getMovieQuery),
            HydrateFallback: Skeleton,
          },
        ],
      },
      {
        path: '/genres',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: (
              <ProtectedRoute allowedRoles={[Role.User, Role.Admin]}>
                <ModifyGenresPage />
              </ProtectedRoute>
            ),
            loader: globalLoader(queryClient, getGenresQuery),
            HydrateFallback: Skeleton,
          },
        ],
      },
      {
        path: '/profiles',
        element: <UserProfilePage />,
        loader: globalLoader(queryClient, getUserProfileQuery),
        HydrateFallback: Skeleton,
      },
      {
        path: '/discussions',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: <DiscussionPage />,
            loader: globalLoader(queryClient, getDiscussionsQuery),
            HydrateFallback: Skeleton,
          },
          {
            path: ':id',
            element: (
              <ProtectedRoute allowedRoles={[Role.User, Role.Admin]}>
                <Chat />
              </ProtectedRoute>
            ),
            // loader: globalLoader(queryClient, getMessagesQuery),
            // HydrateFallback: Skeleton,
          },
        ],
      },

      { path: '*', element: <Navigate to='/404' /> },
      { path: '/404', element: <ErrorPage /> },
    ],
  },
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
      <ReactQueryDevtools position='bottom-right' />
    </QueryClientProvider>
  </StrictMode>
);
