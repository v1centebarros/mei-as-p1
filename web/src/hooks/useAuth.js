import { useState } from 'react';
import {useMutation} from "@tanstack/react-query";
import {doLogin, doRegister} from "../../api/auth.js";

export const useAuth = () => {
    const [token, setToken] = useState(null);


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
        setToken,
        loginMutation,
        registerMutation
    }
}