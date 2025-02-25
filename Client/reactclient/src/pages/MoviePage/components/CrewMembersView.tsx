import { Chip, Stack, Typography } from '@mui/material';
import { CrewMember, CrewRole } from '../../../types/movie';

const groupOnRole = (crew: CrewMember[]): Array<CrewMember[]> =>
  Object.values(
    crew
      ?.sort((x) => x.role)
      ?.reduce((r, o) => {
        (r[o.role] = r[o.role] || []).push(o);
        return r;
      }, Object.create(null))
  );

export function CrewMembersView({ crew }: { crew: CrewMember[] }) {
  const grouped = crew ? groupOnRole(crew) : null;

  return (
    <Stack spacing={2}>
      {grouped?.map((group, role) => (
        <Stack spacing={1} direction={'row'} key={role}>
          <Chip
            label={CrewRole[role + 1]}
            color='secondary'
            sx={{
              fontWeight: 'bold',
            }}
          />
          <Typography variant='h5'>
            {group?.map((member) => member.fullName).join(', ')}
          </Typography>
        </Stack>
      ))}
    </Stack>
  );
}
