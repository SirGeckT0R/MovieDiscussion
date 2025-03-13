import { UserProfile } from '../types/profile';
import { axiosInstance } from './global';

export async function fetchProfile(): Promise<UserProfile> {
  const { data } = await axiosInstance.get(`/api/profiles`);

  return data;
}
