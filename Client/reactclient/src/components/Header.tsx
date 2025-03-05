import { AppBar, Button, Stack, Typography } from '@mui/material';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { Role } from '../types/user';

export function Header() {
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
  };

  return (
    <AppBar
      position='static'
      sx={{ mb: 5, color: '#1d3a45', backgroundColor: '#90AEAD' }}>
      <Stack
        direction='row'
        sx={{
          justifyContent: 'space-between',
          alignItems: 'center',
        }}>
        <Typography
          component={NavLink}
          to='/'
          variant='h3'
          color='inherit'
          sx={{ textDecoration: 'none' }}>
          MovieDiscussion
        </Typography>

        <Stack
          direction='row'
          spacing={5}
          sx={{
            alignItems: 'center',
          }}>
          <Typography
            component={NavLink}
            to='/movies'
            variant='h5'
            color='inherit'
            sx={{ flexGrow: 1, mr: 2, textDecoration: 'none' }}>
            Movies
          </Typography>
          {user.role != Role.Guest ? (
            <>
              <Typography
                variant='h5'
                component={NavLink}
                to='/movies/new'
                color='inherit'
                sx={{ flexGrow: 1, mr: 2, textDecoration: 'none' }}>
                Create
              </Typography>
              <Typography
                component={NavLink}
                to='/profiles'
                variant='h5'
                color='inherit'
                sx={{ flexGrow: 1, mr: 2, textDecoration: 'none' }}>
                Profile
              </Typography>
              <Typography
                component={NavLink}
                to='/discussions'
                variant='h5'
                color='inherit'
                sx={{ flexGrow: 1, mr: 2, textDecoration: 'none' }}>
                Discussions
              </Typography>
            </>
          ) : (
            <></>
          )}

          {user.role == Role.Admin ? (
            <>
              <Typography
                variant='h5'
                component={NavLink}
                to='/genres'
                color='inherit'
                sx={{ flexGrow: 1, mr: 2, textDecoration: 'none' }}>
                Genres
              </Typography>
              <Typography
                variant='h5'
                component={NavLink}
                to='/people'
                color='inherit'
                sx={{ flexGrow: 1, mr: 2, textDecoration: 'none' }}>
                People
              </Typography>
              <Typography
                variant='h5'
                component={NavLink}
                to='/movies/not-approved'
                color='inherit'
                sx={{ flexGrow: 1, mr: 2, textDecoration: 'none' }}>
                Not Approved
              </Typography>
            </>
          ) : (
            <></>
          )}
        </Stack>
        {user.role == Role.Guest ? (
          <Stack
            direction='row'
            spacing={5}
            sx={{
              alignItems: 'center',
            }}>
            <Button
              variant='contained'
              sx={{ bgcolor: '#874F41' }}
              component={NavLink}
              to='/login'>
              Login
            </Button>
            <Button
              variant='contained'
              sx={{ bgcolor: '#874F41' }}
              component={NavLink}
              to='/register'>
              Register
            </Button>
          </Stack>
        ) : (
          <>
            <Button
              color='error'
              variant='contained'
              onClick={handleLogout}
              sx={{ mr: 2 }}>
              Logout
            </Button>
          </>
        )}
      </Stack>
    </AppBar>
  );
}
