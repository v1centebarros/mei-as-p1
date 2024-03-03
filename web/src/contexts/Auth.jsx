import { createContext, useEffect, useContext } from 'react';
import { useAuth } from '../hooks/useAuth';

export const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
    const { login, isLogged } = useAuth();

    useEffect(() => {
        const t = localStorage.getItem('token');
        if (t) {
            login(t);
        }
    }, []);

    const authCtx = {
        login,
        isLogged
    }
        
    return (
        <AuthContext.Provider value={authCtx}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuthContext = () => {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error('useAuthContext must be used within an AuthProvider');
    }

    return context;
}

        
        