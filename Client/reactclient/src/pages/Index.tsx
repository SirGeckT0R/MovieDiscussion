import { Button, Typography } from '@mui/material';
import { NavLink } from 'react-router-dom';

export default function Index() {
  return (
    <Typography id='zero-state' variant='h3' color='info'>
      This is a Movie Discussion App.
      <br />
      Sign in here
      <br />
      <NavLink to='login'>
        <Button variant='contained' color='primary' component='div'>
          Login
        </Button>
      </NavLink>
    </Typography>
  );
}
