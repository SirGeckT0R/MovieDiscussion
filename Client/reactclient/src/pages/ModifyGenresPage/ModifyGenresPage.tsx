import { Stack } from '@mui/material';
import { getGenresQuery } from '../../queries/genresQueries';
import { queryClient } from '../../api/global';
import { CreateGenreForm } from './components/CreateGenreForm';
import { UpdateGenreForm } from './components/UpdateGenreForm';
import { DeleteGenreForm } from './components/DeleteGenreForm';
import { useQuery } from '@tanstack/react-query';

export function ModifyGenresPage() {
  const genresQuery = getGenresQuery();
  const { data: genres } = useQuery(genresQuery);

  const queryInvalidator = () => {
    queryClient.invalidateQueries(genresQuery.queryKey);
  };

  return (
    <Stack style={{ width: 500 }} spacing={6}>
      <CreateGenreForm queryInvalidator={queryInvalidator} />
      <UpdateGenreForm queryInvalidator={queryInvalidator} genres={genres} />
      <DeleteGenreForm queryInvalidator={queryInvalidator} genres={genres} />
    </Stack>
  );
}
