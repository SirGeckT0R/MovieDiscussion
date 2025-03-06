import { QueryClient } from '@tanstack/react-query';
import { LoaderFunctionArgs } from 'react-router';

export const globalLoader =
  (
    queryClient: QueryClient,
    fetcher: (id: string | undefined) => {
      queryKey: Array<string>;
      queryFn: () => Promise<unknown>;
    }
  ) =>
  async ({ params }: LoaderFunctionArgs) => {
    const query = fetcher(params.id);
    queryClient.ensureQueryData(query);
  };

export const emptyLoader =
  (
    queryClient: QueryClient,
    fetcher: () => {
      queryKey: Array<string>;
      queryFn: () => Promise<unknown>;
    }
  ) =>
  async () => {
    const query = fetcher();
    queryClient.ensureQueryData(query);
  };
