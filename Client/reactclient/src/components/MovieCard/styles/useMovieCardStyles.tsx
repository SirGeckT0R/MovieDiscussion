import { Theme } from '@mui/material';
import { makeStyles } from '@mui/styles';

export const useMovieCardStyles = makeStyles((theme: Theme) => ({
  card: {
    position: 'relative',
    borderRadius: 20,
    overflow: 'hidden',
    boxShadow: '4px 3px rgb(211, 211, 211)',
  },
  media: {
    transition: 'transform 0.3s ease-in-out',
    '&:hover': {
      transform: 'scale(1.1)',
    },
  },
  overlay: {
    position: 'absolute',
    bottom: 10,
    left: 0,
    right: 0,
    height: '60%',
    background:
      'linear-gradient(to top, rgba(255,255,255,1) 60%, rgba(255,255,255,0) 100%)',
    display: 'flex',
    alignItems: 'flex-end',
    justifyContent: 'center',
    padding: theme.spacing(2),
  },
  content: {
    color: theme.palette.text.primary,
    backgroundColor: 'transparent',
  },
}));
