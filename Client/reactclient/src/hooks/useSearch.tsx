import { useQuery } from '@tanstack/react-query';

export function useSearch(
  key: string,
  name: string,
  searchFetch: (
    name: string,
    pageSize: number
  ) => Promise<Array<{ id: string; name: string }>>,
  pageSize: number = 3
) {
  return useQuery({
    queryKey: [key, name],
    queryFn: async () => await searchFetch(name, pageSize),
  });
}
