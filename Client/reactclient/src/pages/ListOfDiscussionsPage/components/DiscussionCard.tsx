import { Card, CardContent, Stack, Typography } from '@mui/material';
import { Discussion } from '../../../types/discussion';
import { NavLink } from 'react-router-dom';

export function DiscussionCard({ discussion }: { discussion: Discussion }) {
  return (
    <Card
      component={NavLink}
      to={discussion.id}
      key={discussion.id}
      variant='outlined'
      sx={{ width: 360, height: 600, textDecoration: 'none' }}>
      <CardContent>
        <Stack direction={'column'}>
          <Typography variant='h3' color='info'>
            {discussion?.title}
          </Typography>
          <Typography variant='h4' color='info'>
            Starting at {discussion?.startAt}
          </Typography>
          <Typography variant='h4' color='info'>
            {discussion?.isActive ? 'Active' : 'Not active'}
          </Typography>
        </Stack>
      </CardContent>
    </Card>
  );
}
