import { useState, useCallback, useEffect } from "react";
import { buildUrl, UserAuthenticated, Verbs } from "../utils";
import { apiRoot, apiRoutes } from "../definitions";

export const useLogin = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [token, setToken] = useState<string | null>(localStorage.getItem('jwt'));
    useEffect(() => setIsLoggedIn(UserAuthenticated()), []);

    const login: (username: string) => Promise<void> = useCallback(async (username: string) => {
        const route = buildUrl(apiRoot, apiRoutes.auth, apiRoutes.login);
        setLoading(true);
        try {
            const response = await fetch(route, {
                headers: [
                    ['Content-Type', 'application/json'],
                    ['Accept', 'application/json'],
                ],
                mode: 'cors',
                method: Verbs.Post,
                body: JSON.stringify({ UserName: username }),
            });
            if (!response.ok) throw new Error('Login failed');
            const data = await response.json();
            const jwt = data.token;
            localStorage.setItem('jwt', jwt);
            setToken(jwt);
            setIsLoggedIn(true);
        }
        catch (e) {
            setError('Login failed');
            console.log(e);
            setIsLoggedIn(false);
        }
        finally {
            setLoading(false);
        }
    }, []);

    const logout = () => {
        localStorage.removeItem('jwt');
        setToken(null);
        setIsLoggedIn(false);
    }

    return { login, logout, isLoggedIn, loading, error, token };
}