import { QueryClient } from '@tanstack/react-query';

export const globalLoader =
  (
    queryClient: QueryClient,
    fetcher: { queryKey: Array<string>; queryFn: () => Promise<unknown> }
  ) =>
  async () =>
    queryClient.ensureQueryData(fetcher);
