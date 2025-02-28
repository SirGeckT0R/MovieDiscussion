import { useQuery } from '@tanstack/react-query';
import { fetchWatchlist } from '../api/watchlistService';

export function useWatchlist(isAuthenticated: boolean) {
  return useQuery(['watchlist'], async () => await fetchWatchlist(), {
    enabled: isAuthenticated,
  });
}
