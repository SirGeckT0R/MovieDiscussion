import { Chip, Stack, Typography } from '@mui/material';
import { CrewMember, CrewRole } from '../../types/movie';
import { groupOnRole } from '../../helpers/crewMemberHelper';

export function CrewMembersView({ crew }: { crew: CrewMember[] | undefined }) {
  const grouped = crew ? groupOnRole(crew) : null;

  return (
    <Stack spacing={2}>
      {grouped?.map((group) => (
        <Stack spacing={1} direction={'row'} key={group[0].role}>
          <Chip
            label={CrewRole[group[0].role]}
            color='primary'
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
