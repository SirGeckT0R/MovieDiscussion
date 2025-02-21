import { useQuery } from '@tanstack/react-query';

export function useSearch(
  key: string,
  name: string,
  searchFetch: (name: string) => Promise<Array<{ id: string; name: string }>>
) {
  return useQuery({
    queryKey: [key, name],
    queryFn: async () => await searchFetch(name),
  });
}
