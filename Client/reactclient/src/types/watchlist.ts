import { Movie } from './movie';

export interface Watchlist {
  id: string;
  profileId: string;
  movies: Movie[];
}
