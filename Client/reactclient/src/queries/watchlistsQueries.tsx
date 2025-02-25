import { fetchWatchlist } from '../api/watchlistService';

export const getWatchlistQuery = () => ({
  queryKey: ['watchlist'],
  queryFn: async () => await fetchWatchlist(),
});
