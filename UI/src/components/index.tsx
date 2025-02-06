import { useEffect, useState } from "react";
import { useLogin } from "../hooks/useLogin";
import { ToDoList } from "./ToDoList";

export function ToDoApp() {
  const [username, setUsername] = useState('');

  useEffect(() => localStorage.removeItem('jwt'), []);
  const { login, logout, isLoggedIn, loading, error } = useLogin();

  const renderLogin = () => (
    <>
    {error && <p>{error}</p>}
      <input
        type="text"
        placeholder="User Name"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <button onClick={() => login(username)}>Login</button>
    </>
  )

  const renderApp = () => (
    <>
      <h1>Logged In</h1>
      <button onClick={logout}>Logout</button>
      <ToDoList />
    </>
  )

  return (
    <>
      {
        loading ? <p>Loading...</p> :
        isLoggedIn
          ? renderApp()
          : renderLogin()
      }
    </>
  )
}