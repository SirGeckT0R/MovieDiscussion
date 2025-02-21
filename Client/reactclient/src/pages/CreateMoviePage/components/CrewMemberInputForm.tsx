import { Button, MenuItem, Stack, TextField } from '@mui/material';
import { Dispatch, SetStateAction } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { CrewMember, CrewRole, crewRoles } from '../../../types/movie';
import { fetchPeople } from '../../../api/peopleService';
import { DebounceSearch } from '../../../components/Inputs/DebounceSearch';

export function CrewMemberInputForm({
  crewState,
}: {
  crewState: {
    crew: CrewMember[];
    setCrew: Dispatch<SetStateAction<CrewMember[]>>;
  };
}) {
  const { handleSubmit: handleCrewSubmit, control: crewControl } =
    useForm<CrewMember>({
      defaultValues: {},
    });

  const onCrewSubmit = (formBody: CrewMember) => {
    crewState.setCrew(crewState.crew.concat(formBody));
  };

  return (
    <form onSubmit={handleCrewSubmit(onCrewSubmit)}>
      <Stack style={{ width: 500 }} spacing={2}>
        <DebounceSearch
          inputName='personId'
          control={crewControl}
          searchData={{ key: 'people', searchFetch: fetchPeople }}
        />

        <Controller
          name='role'
          control={crewControl}
          defaultValue={CrewRole.None}
          rules={{ required: 'Role is required' }}
          render={({ field }) => (
            <TextField
              {...field}
              select
              label='Role'
              fullWidth
              defaultValue={''}>
              {crewRoles?.map((role) => (
                <MenuItem key={role} value={role}>
                  {CrewRole[role]}
                </MenuItem>
              ))}
            </TextField>
          )}
        />
        <Button type='submit' variant='contained'>
          Add Crew Member
        </Button>
      </Stack>
    </form>
  );
}
