import { AppBar, Button, Stack, Typography } from '@mui/material';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { Role } from '../types/user';

export function Header() {
  const { role, logout } = useAuth();

  const handleLogout = () => {
    logout();
  };

  return (
    <AppBar
      position='static'
      sx={{ mb: 5, backgroundColor: '#008B8BAA', color: '#FFFFFF' }}>
      <Stack
        direction='row'
        sx={{
          justifyContent: 'space-between',
          alignItems: 'center',
        }}>
        <NavLink to='/'>
          <Typography color='textPrimary' variant='h3'>
            MovieDiscussion
          </Typography>
        </NavLink>

        <Stack
          direction='row'
          spacing={5}
          sx={{
            alignItems: 'center',
          }}>
          <NavLink to='/movies'>
            <Typography
              variant='h5'
              color='textPrimary'
              component='div'
              sx={{ flexGrow: 1, mr: 2 }}>
              Movies
            </Typography>
          </NavLink>
          {role != Role.Guest ? (
            <>
              <NavLink to='/movies/new'>
                <Typography
                  variant='h5'
                  color='textPrimary'
                  component='div'
                  sx={{ flexGrow: 1, mr: 2 }}>
                  Create
                </Typography>
              </NavLink>
            </>
          ) : (
            <></>
          )}

          {role == Role.Admin ? (
            <>
              <NavLink to='/genres'>
                <Typography variant='h5' color='textPrimary' component='div'>
                  Genres
                </Typography>
              </NavLink>
            </>
          ) : (
            <></>
          )}
        </Stack>
        {role == Role.Guest ? (
          <Stack
            direction='row'
            spacing={5}
            sx={{
              alignItems: 'center',
            }}>
            <NavLink to='/login'>
              <Button variant='contained' color='primary' component='div'>
                Login
              </Button>
            </NavLink>
            <NavLink to='/register'>
              <Button variant='contained' color='primary' component='div'>
                Register
              </Button>
            </NavLink>
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
