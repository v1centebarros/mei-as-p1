import {createContext, useContext, useEffect, useMemo} from 'react';
import { useAuth } from '../hooks/useAuth';
import { jwtDecode } from "jwt-decode"

export const AuthContext = createContext({});

export const AuthProvider = ({children}) => {
    const {login, isLogged, token, setToken, logout, role, setRole} = useAuth();

    useEffect(() => {
        const t = localStorage.getItem('token');
        if (t) {
            setToken(() => t)
            setRole(() => jwtDecode(t)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"])
        }
    }, []);

    const authCtx = useMemo(() => {
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
        , [token,role]);

    return <AuthContext.Provider value={authCtx}>
        {children}
    </AuthContext.Provider>
};

export const useAuthContext = () => {
    const context = useContext(AuthContext)
    if (context === undefined) {
        throw new Error('useAuthContext must be used within a AuthProvider')
    }
    return context
}

        
        