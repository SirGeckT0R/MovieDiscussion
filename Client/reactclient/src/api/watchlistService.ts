import { ManageMovieInWatchlist } from '../types/watchlist';
import { axiosInstance } from './global';

export async function fetchWatchlist() {
  try {
    const { data } = await axiosInstance.get(`/api/watchlists`);

    return data;
  } catch {
    return null;
  }
}

export async function manageMovieInWatchlist(body: ManageMovieInWatchlist) {
  const { data } = await axiosInstance.put(`/api/watchlists`, body);

  return data;
}
