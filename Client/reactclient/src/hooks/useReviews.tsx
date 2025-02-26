import { useQuery } from '@tanstack/react-query';
import { fetchReviews } from '../api/reviewService';

export function useReviews(id: string, pageIndex: number) {
  return useQuery({
    queryKey: ['reviews', id + '' + pageIndex],
    queryFn: async () => await fetchReviews(id, pageIndex),
  });
}
