import { useQuery } from '@tanstack/react-query';

export function useSearch(
  key: string,
  name: string,
  searchFetch: (
    name: string,
    pageIndex: number
  ) => Promise<Array<{ id: string; name: string }>>,
  pageIndex: number
) {
  return useQuery({
    queryKey: [key, name],
    queryFn: async () => await searchFetch(name, pageIndex),
  });
}
