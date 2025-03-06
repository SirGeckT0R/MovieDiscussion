import { fetchWatchlist } from '../api/watchlistService';

export const getWatchlistQuery = (isAuthenticated: boolean = false) => ({
  queryKey: ['watchlist'],
  queryFn: async () => await fetchWatchlist(),
  options: {
    enabled: isAuthenticated,
  },
});
