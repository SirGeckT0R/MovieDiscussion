import {
  CreatePersonRequest,
  DeletePersonRequest,
  PaginatedPerson,
  Person,
  UpdatePersonRequest,
} from '../types/people';
import { axiosInstance } from './global';

export const fetchPeople = async (
  searchName?: string,
  pageSize?: number
): Promise<Person[]> => {
  const { data }: { data: PaginatedPerson } = await axiosInstance.get(
    `/api/people?Name=${searchName?.trim() ?? ''}&PageSize=${pageSize}`
  );

  data.items?.forEach((person) => {
    person.name = person.firstName + ' ' + person.lastName;
  });

  return data.items;
};

export const createPerson = async (body: CreatePersonRequest) => {
  const { data } = await axiosInstance.postForm('/api/people', body);

  return data;
};

export const updatePerson = async (body: UpdatePersonRequest) => {
  const { data } = await axiosInstance.putForm(`/api/people/${body.id}`, body);

  return data;
};

export const deletePerson = async (body: DeletePersonRequest) => {
  const { data } = await axiosInstance.delete(`/api/people/${body.id}`);

  return data;
};
