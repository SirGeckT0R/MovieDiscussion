import { Card, CardContent, Stack, Typography } from '@mui/material';
import { Discussion } from '../../../types/discussion';
import { NavLink } from 'react-router-dom';
import { DateDisplay } from '../../../components/MovieFieldsView/DateDisplay';

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
            Starting at
          </Typography>
          <DateDisplay date={new Date(discussion?.startAt)} showTime={true} />
          <Typography variant='h4' color='info'>
            {discussion?.isActive ? 'Active' : 'Not active'}
          </Typography>
        </Stack>
      </CardContent>
    </Card>
  );
}
