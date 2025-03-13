import { Movie } from './movie';

export interface Watchlist {
  id: string;
  profileId: string;
  movies: Movie[];
}

export enum WatchlistAction {
  None = 0,
  Add = 1,
  Remove = 2,
}

export interface ManageMovieInWatchlist {
  movieId: string;
  action: number;
}
