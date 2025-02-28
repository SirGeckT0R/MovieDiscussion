import {
  CreateMovieRequest,
  CrewMember,
  Movie,
  PaginatedMovies,
  UpdateMovieRequest,
} from '../types/movie';
import { axiosInstance } from './global';

export const fetchMovies = async (
  pageIndex: number = 1,
  searchName: string | null = null
): Promise<PaginatedMovies> => {
  const movies: PaginatedMovies = await axiosInstance
    .get(
      `/api/movies?Name=${
        searchName ? searchName.trim() : ''
      }&PageIndex=${pageIndex}&PageSize=5`
    )
    .then((response) => response.data);

  return movies;
};

export const fetchMovie = async (movieId: string): Promise<Movie> => {
  const movie: Movie = await axiosInstance
    .get(`/api/movies/${movieId}`)
    .then((response) => response.data);

  return movie;
};

export const createMovie = async (
  body: CreateMovieRequest,
  image: Blob | null,
  crew: CrewMember[]
) => {
  body.crewMembers = crew;
  if (image != null) {
    body.file = image;
  }

  const response = await axiosInstance
    .postForm('/api/movies', body)
    .then((response) => response.data);

  return response;
};

export const updateMovie = async (
  body: UpdateMovieRequest,
  image: Blob | null,
  crew: CrewMember[]
) => {
  body.crewMembers = crew;
  if (image != null) {
    body.file = image;
  }

  const response = await axiosInstance
    .putForm(`/api/movies/${body.id}`, body)
    .then((response) => response.data);

  return response;
};

export const deleteMovie = async (id: string, image: string | null) => {
  const response = await axiosInstance
    .delete(`/api/movies/${id}`, { data: { image: image } })
    .then((response) => response.data);

  return response;
};
