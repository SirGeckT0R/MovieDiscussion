import { createTheme, ThemeProvider } from '@mui/material';
import './App.css';
import { Toaster } from 'react-hot-toast';
import { Outlet } from 'react-router-dom';

const theme = createTheme({
  palette: {
    primary: {
      main: '#90AEAD',
    },
    secondary: {
      main: '#f3e7d6',
    },
    error: {
      main: '#E64833',
    },
    warning: {
      main: '#874F41',
    },
    info: {
      main: '#1d3a45',
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
