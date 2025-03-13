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
  const { data } = await axiosInstance.get('/api/movies', {
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
  });

  return data;
};

export const fetchMovie = async (movieId: string): Promise<Movie> => {
  const { data } = await axiosInstance.get(`/api/movies/${movieId}`);

  return data;
};

export const fetchNotApprovedMovies = async (): Promise<Movie[]> => {
  const { data } = await axiosInstance.get(`/api/movies/not-approved`);

  return data;
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

  const { data } = await axiosInstance.postForm('/api/movies', body);

  return data;
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

  const { data } = await axiosInstance.putForm(`/api/movies/${body.id}`, body);

  return data;
};

export const deleteMovie = async (body: DeleteMovieRequest) => {
  const { data } = await axiosInstance.delete(`/api/movies/${body.id}`, {
    data: { image: body.image },
  });

  return data;
};

export const manageMovieApproval = async (body: ManageMovieApprovalRequest) => {
  const { data } = await axiosInstance.put(
    `/api/movies/not-approved/${body.movieId}`,
    body
  );

  return data;
};
