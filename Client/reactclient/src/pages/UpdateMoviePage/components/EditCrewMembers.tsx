import { Chip, IconButton, Stack, Typography } from '@mui/material';
import { CrewMember, CrewRole } from '../../../types/movie';
import { Dispatch, SetStateAction } from 'react';
import { RemoveCircle } from '@mui/icons-material';

const groupOnRole = (crew: CrewMember[]): Array<CrewMember[]> =>
  Object.values(
    crew
      ?.sort((x) => x.role)
      ?.reduce((r, o) => {
        (r[o.role] = r[o.role] || []).push(o);
        return r;
      }, Object.create(null))
  );

export function EditCrewMembers({
  crew,
  setCrew,
}: {
  crew: CrewMember[];
  setCrew: Dispatch<SetStateAction<CrewMember[]>>;
}) {
  const grouped = groupOnRole(crew);

  const handleDelete = (member: CrewMember) => {
    const index = crew.findIndex(
      ({ personId }) => personId === member.personId
    );
    if (index > -1) {
      setCrew((crew) => [...crew.slice(0, index), ...crew.slice(index + 1)]);
    }
  };

  return (
    <Stack spacing={2}>
      {crew
        ? grouped?.map((group, role) => (
            <Stack spacing={1} direction={'row'} key={role}>
              <Chip
                label={CrewRole[role + 1]}
                color='primary'
                sx={{
                  fontWeight: 'bold',
                }}
              />
              {group?.map((member) => (
                <Typography variant='h5' key={member.personId}>
                  {member.fullName}
                  <IconButton onClick={() => handleDelete(member)}>
                    <RemoveCircle />
                  </IconButton>
                </Typography>
              ))}
            </Stack>
          ))
        : null}
    </Stack>
  );
}
