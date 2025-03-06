import {
  CreateMovieRequest,
  CrewMember,
  DeleteMovieRequest,
  ManageMovieApprovalRequest,
  Movie,
  MovieFilters,
  PaginatedMovies,
  UpdateMovieRequest,
} from '../types/movie';
import { axiosInstance } from './global';

export const fetchMovies = async (
  pageIndex: number = 1,
  filters: MovieFilters | null = null
): Promise<PaginatedMovies> => {
  const movies: PaginatedMovies = await axiosInstance
    .get('/api/movies', {
      params: {
        'Filters.Name': filters?.name?.trim() ?? '',
        'Filters.Genres': filters?.genres ?? [],
        'Filters.CrewMember': filters?.crewMember ?? '',
        PageIndex: pageIndex,
        PageSize: 6,
      },
      paramsSerializer: {
        indexes: null,
      },
    })
    .then((response) => response.data);

  return movies;
};

export const fetchMovie = async (movieId: string): Promise<Movie> => {
  const movie: Movie = await axiosInstance
    .get(`/api/movies/${movieId}`)
    .then((response) => response.data);

  return movie;
};

export const fetchNotApprovedMovies = async (): Promise<Movie[]> => {
  const movies: Movie[] = await axiosInstance
    .get(`/api/movies/not-approved`)
    .then((response) => response.data);

  return movies;
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

export const deleteMovie = async (body: DeleteMovieRequest) => {
  const response = await axiosInstance
    .delete(`/api/movies/${body.id}`, { data: { image: body.image } })
    .then((response) => response.data);

  return response;
};

export const manageMovieApproval = async (body: ManageMovieApprovalRequest) => {
  const response = await axiosInstance
    .put(`/api/movies/not-approved/${body.movieId}`, body)
    .then((response) => response.data);

  return response;
};
