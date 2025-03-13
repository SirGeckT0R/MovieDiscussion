import {
  CreateGenreRequest,
  DeleteGenreRequest,
  Genre,
  UpdateGenreRequest,
} from '../types/genre';
import { axiosInstance } from './global';

export const fetchGenres = async (): Promise<Genre[]> => {
  const { data } = await axiosInstance.get('/api/genres');

  return data;
};

export const createGenre = async (body: CreateGenreRequest) => {
  const { data } = await axiosInstance.postForm('/api/genres', body);

  return data;
};

export const updateGenre = async (body: UpdateGenreRequest) => {
  const { data } = await axiosInstance.putForm(`/api/genres/${body.id}`, body);

  return data;
};

export const deleteGenre = async (body: DeleteGenreRequest) => {
  const { data } = await axiosInstance.delete(`/api/genres/${body.id}`);

  return data;
};
