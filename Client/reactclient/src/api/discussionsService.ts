import {
  CreateDiscussionRequest,
  DeleteDiscussionRequest,
  Discussion,
  UpdateDiscussionRequest,
} from '../types/discussion';
import { axiosInstance } from './global';

export const fetchListOfDiscussions = async (): Promise<Discussion[]> => {
  const { data } = await axiosInstance.get('/api/discussions');

  return data;
};

export const fetchDiscussion = async (id: string): Promise<Discussion> => {
  const { data } = await axiosInstance.get(`/api/discussions/${id}`);

  return data;
};

export const createDiscussion = async (body: CreateDiscussionRequest) => {
  body.createdBy = null;
  const { data } = await axiosInstance.post('/api/discussions', body);

  return data;
};

export const updateDiscussion = async (body: UpdateDiscussionRequest) => {
  body.updatedBy = null;
  const { data } = await axiosInstance.put(`/api/discussions/${body.id}`, body);

  return data;
};

export const deleteDiscussion = async (body: DeleteDiscussionRequest) => {
  const { data } = await axiosInstance.delete(`/api/discussions/${body.id}`);

  return data;
};

export const subscribeToDiscussion = async (id: string) => {
  const { data } = await axiosInstance.post(
    `/api/discussions/${id}/subscribers`
  );

  return data;
};
