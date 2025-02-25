import {
  CreateMovieRequest,
  CrewMember,
  Movie,
  PaginatedMovie,
  UpdateMovieRequest,
} from '../types/movie';
import { axiosInstance } from './global';

export const fetchMovies = async (
  pageIndex: number = 1,
  searchName: string | null = null
): Promise<PaginatedMovie> => {
  const movies: PaginatedMovie = await axiosInstance
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

// export const deleteMovie = async (
//   body: UpdateMovieRequest,
//   image: Blob | null,
//   crew: CrewMember[]
// ) => {
//   body.crewMembers = crew;
//   if (image != null) {
//     body.file = image;
//   }

//   const response = await axiosInstance
//     .delete(`/api/movies/${body.id}`, body)
//     .then((response) => response.data);

//   return response;
// };
