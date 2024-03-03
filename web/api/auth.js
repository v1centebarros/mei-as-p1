import {api} from "./axio.js";

export const doRegister = async (user) => {
    const response = await api.post('/Auth/register', user);

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }
    return response.data;
}

export const doLogin = async (user) => {
    const response = await api.post('/Auth/login', user);

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }

    return response.data;
}
