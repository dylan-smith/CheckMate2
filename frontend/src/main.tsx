import { StrictMode } from 'react'
import CssBaseline from '@mui/material/CssBaseline'
import { ThemeProvider, createTheme } from '@mui/material/styles'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'

const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#9c27b0',
    },
    background: {
      default: '#f3f4f6',
    },
    text: {
      primary: '#111827',
    },
  },
  typography: {
    fontFamily: 'Inter, system-ui, -apple-system, Segoe UI, Roboto, sans-serif',
  },
})

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <App />
    </ThemeProvider>
  </StrictMode>,
)
