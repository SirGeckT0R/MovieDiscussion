import { fetchProfile } from '../api/userProfileService';

export const getUserProfileQuery = () => ({
  queryKey: ['profile'],
  queryFn: async () => await fetchProfile(),
});
