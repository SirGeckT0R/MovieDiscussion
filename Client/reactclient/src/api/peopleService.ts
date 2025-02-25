import { CreatePersonRequest, Person } from '../types/people';
import { axiosInstance } from './global';

export const fetchPeople = async (searchName: string): Promise<Person[]> => {
  const people: Person[] = await axiosInstance
    .get(`/api/people?Name=${searchName ? searchName.trim() : ''}&PageSize=3`)
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
