import {
  CreateDiscussionRequest,
  Discussion,
  UpdateDiscussionRequest,
} from '../types/discussion';
import { axiosInstance } from './global';

export const fetchListOfDiscussions = async (): Promise<Discussion[]> => {
  const discussions: Discussion[] = await axiosInstance
    .get('/api/discussions')
    .then((response) => {
      return response.data;
    });

  return discussions;
};

export const fetchDiscussion = async (id: string): Promise<Discussion> => {
  const discussion: Discussion = await axiosInstance
    .get(`/api/discussions/${id}`)
    .then((response) => {
      return response.data;
    });

  return discussion;
};

export const createDiscussion = async (body: CreateDiscussionRequest) => {
  body.createdBy = null;
  const response = await axiosInstance
    .post('/api/discussions', body)
    .then((response) => {
      return response.data;
    });

  return response;
};

export const updateDiscussion = async (body: UpdateDiscussionRequest) => {
  body.updatedBy = null;
  const response = await axiosInstance
    .put(`/api/discussions/${body.id}`, body)
    .then((response) => {
      return response.data;
    });

  return response;
};

export const deleteDiscussion = async (id: string) => {
  const response = await axiosInstance
    .delete(`/api/discussions/${id}`)
    .then((response) => {
      return response.data;
    });

  return response;
};

export const subscribeToDiscussion = async (id: string) => {
  const response = await axiosInstance
    .post(`/api/discussions/${id}/subscribers`)
    .then((response) => {
      return response.data;
    });

  return response;
};
