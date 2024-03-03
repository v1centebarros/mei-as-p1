import {createContext, useContext, useEffect, useMemo} from 'react';
import {useAuth} from '../hooks/useAuth';
import {useMutation} from "@tanstack/react-query";
import {doLogin, doRegister} from "../../api/auth.js";

export const AuthContext = createContext({});

export const AuthProvider = ({children}) => {
    const {login, isLogged, token, setToken,logout} = useAuth();

    useEffect(() => {
        const t = localStorage.getItem('token');
        if (t) {
            setToken(()=>t)
        }
    }, []);

    const loginMutation = useMutation({
        mutationFn: (userData) => doLogin(userData),
        onSuccess: (data) => {
            setToken(()=>data.token)
            localStorage.setItem('token', data.token)
        },
        onError: (error) => {
            console.log('error', error)
        }
    });

    const registerMutation = useMutation({
        mutationFn:(userData) => doRegister(userData),
        onSuccess: (data) => {
            console.log('data', data)
        },
        onError: (error) => {
            console.log('error', error)
        }
    })

    const authCtx = useMemo(() => {
            return {
                login,
                logout,
                isLogged,
                token,
                setToken,
                loginMutation,
                registerMutation
            }
        }
        , [token]);

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

        
        