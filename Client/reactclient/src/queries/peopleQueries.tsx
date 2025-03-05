import { fetchPeople } from '../api/peopleService';

export const getPeople = () => {
  return { queryKey: ['people'], queryFn: async () => await fetchPeople() };
};
