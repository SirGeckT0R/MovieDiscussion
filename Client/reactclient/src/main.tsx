import { createRoot } from 'react-dom/client';
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
import { emptyLoader, globalLoader } from './loaders/loader.tsx';
import { AuthProvider } from './providers/AuthProvider.tsx';
import { Role } from './types/user.ts';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import {
  getListOfMoviesQuery,
  getMovieQuery,
  getNotApprovedMoviesQuery,
} from './queries/moviesQueries.tsx';
import { CardLoader } from './components/Loaders/CardLoader.tsx';
import { CreateMoviePage } from './pages/CreateMoviePage/CreateMoviePage.tsx';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { getGenresQuery } from './queries/genresQueries.tsx';
import { Skeleton } from '@mui/material';
import { LoginPage } from './pages/LoginPage/LoginPage.tsx';
import { RegisterPage } from './pages/RegisterPage/RegisterPage.tsx';
import { ListOfMoviesPage } from './pages/ListOfMoviesPage/ListOfMoviesPage.tsx';
import { ModifyGenresPage } from './pages/ModifyGenresPage/ModifyGenresPage.tsx';
import { Header } from './components/Header/Header.tsx';
import { MoviePage } from './pages/MoviePage/MoviePage.tsx';
import { UpdateMoviePage } from './pages/UpdateMoviePage/UpdateMoviePage.tsx';
import { UserProfilePage } from './pages/UserProfilePage/UserProfilePage.tsx';
import { getUserProfileQuery } from './queries/profilesQueries.tsx';
import { Chat } from './pages/Chat/Chat.tsx';
import { ListOfDiscussionsPage } from './pages/ListOfDiscussionsPage/ListOfDiscussionsPage.tsx';
import {
  getDiscussionQuery,
  getListOfDiscussionsQuery,
} from './queries/discussionsQueries.tsx';
import { DiscussionPage } from './pages/DiscussionPage/DiscussionPage.tsx';
import { EditDiscussionPage } from './pages/EditDiscussionPage/EditDiscussionPage.tsx';
import { getMessagesQuery } from './queries/messagesQueries.tsx';
import { ConfirmEmailPage } from './pages/ConfirmEmailPage/ConfirmEmailPage.tsx';
import { ResetPasswordPage } from './pages/ResetPasswordPage/ResetPasswordPage.tsx';
import { ForgotPasswordPage } from './pages/ForgotPasswordPage/ForgotPasswordPage.tsx';
import { ModifyPersonPage } from './pages/ModifyPersonPage/ModifyPersonPage.tsx';
import { NotApprovedMoviesPage } from './pages/NotApprovedMoviesPage/NotApprovedMoviesPage.tsx';
import { ProtectedRoute } from './components/ProtectedRoute/ProtectedRoute.tsx';

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
            loader: emptyLoader(queryClient, getListOfMoviesQuery),
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
            path: 'not-approved',
            element: (
              <ProtectedRoute allowedRoles={[Role.Admin]}>
                <NotApprovedMoviesPage />
              </ProtectedRoute>
            ),
            loader: globalLoader(queryClient, getNotApprovedMoviesQuery),
            HydrateFallback: CardLoader,
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
              <ProtectedRoute allowedRoles={[Role.Admin]}>
                <ModifyGenresPage />
              </ProtectedRoute>
            ),
            loader: globalLoader(queryClient, getGenresQuery),
            HydrateFallback: Skeleton,
          },
        ],
      },
      {
        path: '/people',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: (
              <ProtectedRoute allowedRoles={[Role.Admin]}>
                <ModifyPersonPage />
              </ProtectedRoute>
            ),
          },
        ],
      },
      {
        path: '/profiles',
        element: (
          <ProtectedRoute allowedRoles={[Role.User, Role.Admin]}>
            <UserProfilePage />
          </ProtectedRoute>
        ),
        loader: globalLoader(queryClient, getUserProfileQuery),
        HydrateFallback: Skeleton,
      },
      {
        path: '/discussions',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: <ListOfDiscussionsPage />,
            loader: globalLoader(queryClient, getListOfDiscussionsQuery),
            HydrateFallback: Skeleton,
          },
          {
            path: ':id',
            children: [
              {
                index: true,
                element: <DiscussionPage />,
                loader: globalLoader(queryClient, getDiscussionQuery),
                HydrateFallback: Skeleton,
              },
              {
                path: 'chat',
                element: (
                  <ProtectedRoute allowedRoles={[Role.User, Role.Admin]}>
                    <Chat />
                  </ProtectedRoute>
                ),
                loader: globalLoader(queryClient, getMessagesQuery),
                HydrateFallback: Skeleton,
              },
              {
                path: 'edit',
                element: (
                  <ProtectedRoute allowedRoles={[Role.User, Role.Admin]}>
                    <EditDiscussionPage />
                  </ProtectedRoute>
                ),
                loader: globalLoader(queryClient, getDiscussionQuery),
                HydrateFallback: Skeleton,
              },
            ],
          },
        ],
      },
      { path: '/confirmation', element: <ConfirmEmailPage /> },
      { path: '/auth/password/reset', element: <ResetPasswordPage /> },
      { path: '/auth/password/forgot', element: <ForgotPasswordPage /> },

      { path: '*', element: <Navigate to='/404' /> },
      { path: '/404', element: <ErrorPage /> },
    ],
  },
]);

createRoot(document.getElementById('root')!).render(
  <QueryClientProvider client={queryClient}>
    <RouterProvider router={router} />
    <ReactQueryDevtools position='bottom' />
  </QueryClientProvider>
);
