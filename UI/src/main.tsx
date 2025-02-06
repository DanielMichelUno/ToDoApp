import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { ToDoApp } from './components/index.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ToDoApp />
  </StrictMode>,
)
