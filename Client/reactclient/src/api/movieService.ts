import { CreateMovieRequest, CrewMember, Movie } from '../types/movie';
import { axiosInstance } from './global';

export const fetchMovies = async (): Promise<Movie[]> => {
  const movies: Movie[] = await axiosInstance
    .get('/api/movies')
    .then((response) => response.data);

  return movies;
};

export const createMovie = async (
  body: CreateMovieRequest,
  image: Blob | null,
  crew: CrewMember[]
) => {
  body.crewMembers = crew;
  body.file = image;
  body.image = 'placeholder';

  const response = await axiosInstance
    .postForm('/api/movies', body)
    .then((response) => response.data);

  return response;
};
