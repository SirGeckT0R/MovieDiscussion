import { Button, Stack, TextField } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { Dispatch, SetStateAction } from 'react';
import { useForm } from 'react-hook-form';
import { getGenresQuery } from '../../../queries/genresQueries';
import { CustomSelectInput } from '../../../components/Inputs/MultipleSelectInput';
import { MovieFilters } from '../../../types/movie';
import { DebounceSearch } from '../../../components/Inputs/DebounceSearch';
import { fetchPeople } from '../../../api/peopleService';

type Props = {
  setFilters: Dispatch<SetStateAction<MovieFilters>>;
};

export function MovieFiltersForm({ setFilters }: Props) {
  const genresQuery = getGenresQuery();
  const { data: genres } = useQuery(genresQuery);
  const { register, handleSubmit, control } = useForm<MovieFilters>();

  const onFiltersSubmit = (values: MovieFilters) => {
    setFilters(values);
  };

  return (
    <form onSubmit={handleSubmit(onFiltersSubmit)}>
      <Stack spacing={2} sx={{ width: 500, margin: '0 auto' }}>
        <TextField
          type='text'
          id='nameInput'
          label='Title'
          {...register('name')}
        />
        <CustomSelectInput
          isMultiple={true}
          options={genres}
          label='Genres'
          control={control}
          inputName='genres'
          isRequired={false}
        />
        <DebounceSearch
          inputName='crewMember'
          control={control}
          searchData={{ key: 'people', searchFetch: fetchPeople }}
          isRequired={false}
        />
        <Button type='submit' variant='contained'>
          Apply
        </Button>
      </Stack>
    </form>
  );
}
