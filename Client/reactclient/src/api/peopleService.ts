import {
  CreatePersonRequest,
  DeletePersonRequest,
  Person,
  UpdatePersonRequest,
} from '../types/people';
import { axiosInstance } from './global';

export const fetchPeople = async (
  searchName?: string,
  pageSize?: number
): Promise<Person[]> => {
  const people: Person[] = await axiosInstance
    .get(
      `/api/people?Name=${
        searchName ? searchName.trim() : ''
      }&PageSize=${pageSize}`
    )
    .then((response) => response.data.items)
    .then((array: Person[]) => {
      array?.forEach((person) => {
        person.name = person.firstName + ' ' + person.lastName;
      });
      return array;
    });

  return people;
};

export const createPerson = async (body: CreatePersonRequest) => {
  const response = await axiosInstance
    .postForm('/api/people', body)
    .then((response) => response.data);

  return response;
};

export const updatePerson = async (body: UpdatePersonRequest) => {
  const response = await axiosInstance
    .putForm(`/api/people/${body.id}`, body)
    .then((response) => response.data);

  return response;
};

export const deletePerson = async (body: DeletePersonRequest) => {
  const response = await axiosInstance
    .delete(`/api/people/${body.id}`)
    .then((response) => response.data);

  return response;
};
