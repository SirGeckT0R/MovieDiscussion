import { createTheme, ThemeProvider } from '@mui/material';
import './App.css';
import { Toaster } from 'react-hot-toast';
import { Outlet } from 'react-router-dom';

const theme = createTheme({
  palette: {
    primary: {
      main: '#FBE9D0',
    },
    secondary: {
      main: '#90AEAD',
    },
    error: {
      main: '#E64833',
    },
    warning: {
      main: '#874F41',
    },
    info: {
      main: '#244855',
    },
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <div>
        <div style={{ display: 'inline-block' }}>
          <Outlet />
        </div>
        <Toaster />
      </div>
    </ThemeProvider>
  );
}

export default App;
