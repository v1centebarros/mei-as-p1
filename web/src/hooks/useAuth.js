import { useState } from 'react';

export const useAuth = () => {
    const [token, setToken] = useState(null);

    const login = (token) => {
        setToken(token);
    }

    const logout = () => {
        setToken(null);
        localStorage.removeItem('token');
    }

    const isLogged = () => {
       return token !== null;
    }

    return {
        login,
        logout,
        isLogged,
        token,
        setToken
    }
}