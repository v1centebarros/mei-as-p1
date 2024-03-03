import { useState } from 'react';

export const useAuth = () => {
    const [token, setToken] = useState(null);

    const login = (t) => {
        setToken(t);
        localStorage.setItem('token', t);
    }

    const logout = () => {
        setToken(null);
        localStorage.removeItem('token');
    }

    const isLogged = () => {
        return token !== null;
    }

    return {
        token,
        login,
        logout,
        isLogged
    }
}