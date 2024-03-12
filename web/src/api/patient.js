import {api} from "./axio.js";

export const getMe = async (token) => {
    const response = await api.get('/Patient/Me', {
        headers: {
            Authorization: "Bearer " + token
        }
    });

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }

    return response.data;
}