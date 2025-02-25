import {
  CreateGenreRequest,
  DeleteGenreRequest,
  Genre,
  UpdateGenreRequest,
} from '../types/genre';
import { axiosInstance } from './global';

export const fetchGenres = async (): Promise<Genre[]> => {
  const genres: Genre[] = await axiosInstance
    .get('/api/genres')
    .then((response) => response.data);

  return genres;
};

export const createGenre = async (body: CreateGenreRequest) => {
  const response = await axiosInstance
    .postForm('/api/genres', body)
    .then((response) => response.data);

  return response;
};

export const updateGenre = async (body: UpdateGenreRequest) => {
  const response = await axiosInstance
    .putForm(`/api/genres/${body.id}`, body)
    .then((response) => response.data);

  return response;
};

export const deleteGenre = async (body: DeleteGenreRequest) => {
  const response = await axiosInstance
    .delete(`/api/genres/${body.id}`)
    .then((response) => response.data);

  return response;
};
