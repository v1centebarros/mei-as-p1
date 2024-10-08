import { useState } from 'react';

export const useAuth = () => {
    const [token, setToken] = useState(null);
    const [role, setRole] = useState(null);
    const login = (token) => {
        setToken(token);
    }

    const logout = () => {
        setToken(null);
        setRole(null);
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
        setToken,
        role,
        setRole
    }
}