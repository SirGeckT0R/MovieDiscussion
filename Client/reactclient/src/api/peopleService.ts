import { Person } from '../types/people';
import { axiosInstance } from './global';

export const fetchPeople = async (searchName: string): Promise<Person[]> => {
  const people: Person[] = await axiosInstance
    .get(`/api/people?Name=${searchName ? searchName.trim() : ''}&PageSize=3`)
    .then((response) => response.data)
    .then((array: Person[]) => {
      array?.forEach((person) => {
        person.name = person.firstName + ' ' + person.lastName;
      });
      return array;
    });

  return people;
};
