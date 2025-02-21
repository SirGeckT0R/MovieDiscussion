import { createTheme, ThemeProvider } from '@mui/material';
import './App.css';
import { Toaster } from 'react-hot-toast';
import { Outlet } from 'react-router-dom';

const theme = createTheme();

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
