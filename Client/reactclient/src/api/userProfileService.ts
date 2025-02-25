import { UserProfile } from '../types/profile';
import { axiosInstance } from './global';

export async function fetchProfile() {
  const response: UserProfile = await axiosInstance
    .get(`/api/profiles`)
    .then((response) => response?.data);

  return response;
}
