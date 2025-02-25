import { Watchlist } from '../types/watchlist';
import { axiosInstance } from './global';

export async function fetchWatchlist() {
  const response: Watchlist = await axiosInstance
    .get(`/api/watchlists`)
    .then((response) => response?.data);

  return response;
}
