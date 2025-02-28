import { MutationCache, QueryCache, QueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { ApiError } from '../types/global';
import toast from 'react-hot-toast';

const handleGlobalError = (error: unknown) => {
  const axiosError = error as AxiosError<ApiError>;

  const message = `${axiosError.response?.data.detail ?? axiosError.message}`;

  toast.error(message);
};

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 1,
    },
  },
  queryCache: new QueryCache({
    onError: handleGlobalError,
  }),
  mutationCache: new MutationCache({
    onError: handleGlobalError,
  }),
});

const host = import.meta.env.VITE_WEB_API_HOST;

export const axiosInstance = axios.create({
  baseURL: host,
  withCredentials: true,
});
