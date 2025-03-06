import { ManageMovieInWatchlist, Watchlist } from '../types/watchlist';
import { axiosInstance } from './global';

export async function fetchWatchlist() {
  const response: Watchlist = await axiosInstance
    .get(`/api/watchlists`)
    .then((response) => response?.data)
    .catch(() => null);

  return response;
}

export async function manageMovieInWatchlist(body: ManageMovieInWatchlist) {
  const response = await axiosInstance
    .put(`/api/watchlists`, body)
    .then((response) => response?.data);

  return response;
}
