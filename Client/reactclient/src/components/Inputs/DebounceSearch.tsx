import { Autocomplete, CircularProgress, TextField } from '@mui/material';
import { useDebounce } from '@uidotdev/usehooks';
import {
  Dispatch,
  ReactElement,
  SetStateAction,
  SyntheticEvent,
  useState,
} from 'react';
import { Control, Controller } from 'react-hook-form';
import { useSearch } from '../../hooks/useSearch';

export function DebounceSearch({
  searchData,
  control,
  inputName,
  noOptionsRender,
  setName,
}: {
  searchData: {
    key: string;
    searchFetch: (name: string) => Promise<Array<{ id: string; name: string }>>;
  };
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<any, any>;
  inputName: string;
  noOptionsRender: ReactElement | undefined;
  setName: Dispatch<SetStateAction<string>>;
}) {
  const [searchName, setSearchName] = useState<string | null>();
  const debouncedSearchTerm = useDebounce(searchName, 1000);

  const { data: matches, isLoading } = useSearch(
    searchData.key,
    debouncedSearchTerm!,
    searchData.searchFetch
  );

  return (
    <Controller
      name={inputName}
      control={control}
      defaultValue={''}
      rules={{ required: `${inputName} is required` }}
      render={({ field, fieldState: { error } }) => (
        <Autocomplete
          noOptionsText={
            isLoading ? (
              <CircularProgress />
            ) : (
              noOptionsRender ?? 'No options found'
            )
          }
          {...field}
          defaultValue={{ id: '', name: '' }}
          options={matches ?? [{ id: '', name: '' }]}
          getOptionLabel={(option) => option.name}
          onInputChange={(_event: SyntheticEvent, value: string | null) =>
            setSearchName(value!)
          }
          onChange={(
            _event: SyntheticEvent,
            newValue: { id: string; name: string } | null
          ) => {
            setName(newValue?.name ?? '');
            field.onChange(newValue ? newValue.id : '');
          }}
          value={
            matches?.find((option) => option.id === field.value)
              ? {
                  id: field.value,
                  name: `${
                    matches.find((option) => option.id === field.value)?.name
                  }`,
                }
              : null
          }
          renderInput={(params) => (
            <TextField
              error={!!error}
              helperText={error ? error.message : null}
              {...params}
            />
          )}
        />
      )}
    />
  );
}
