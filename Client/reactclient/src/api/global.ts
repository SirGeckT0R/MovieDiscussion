import { MutationCache, QueryCache, QueryClient } from '@tanstack/react-query';
import axios, { isAxiosError } from 'axios';
import toast from 'react-hot-toast';

const handleGlobalError = (error: unknown) => {
  const isAxios = isAxiosError(error);

  const message = isAxios ? error.response?.data.detail : error;

  toast.error(message);
};

const staleTime = 1000 * 1;

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime,
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
