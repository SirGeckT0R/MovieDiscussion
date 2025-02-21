import { makeStyles } from '@mui/styles';

export const useMovieCardStyles = makeStyles((theme) => ({
  card: {
    position: 'relative',
    borderRadius: theme.shape.borderRadius,
    overflow: 'hidden',
  },
  media: {
    transition: 'transform 0.3s ease-in-out',
    '&:hover': {
      transform: 'scale(1.1)',
    },
  },
  overlay: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    height: '60%',
    background:
      'linear-gradient(to top, rgba(255,255,255,1) 60%, rgba(255,255,255,0) 100%)',
    display: 'flex',
    alignItems: 'flex-end',
    padding: theme.spacing(2),
  },
  content: {
    color: theme.palette.text.primary,
    backgroundColor: 'transparent',
  },
}));
