import { api } from "./axio.js";


export const getPatient = async (token, id) => {
    const response = await api.get('/Patient/' + id, {
        headers: {
            Authorization: "Bearer " + token
        }
    });

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }

    return response.data;
}

export const getPatients = async (token) => {
    const response = await api.get('/Patient', {
        headers: {
            Authorization: "Bearer " + token
        }
    });

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }

    return response.data;
}

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

export const editMe = async (patient, token) => {
    const response = await api.put("/Patient", patient,
        {
            headers: {
                Authorization: "Bearer " + token
            }
        });

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }

    return response.data;
}

export const updateWithoutAccessToken = async (token, patient,id) => {
    const response = await api.put("/Patient/" + id,
        patient,
        {
            headers: {
                Authorization: "Bearer " + token
            }
        })

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }

    return response.data;

}


export const updateWithAccessToken = async (token, patient,id, accessToken) => {
    const response = await api.put(`/Patient/${id}/AccessToken?accessToken=${accessToken}`,
        patient,
        {
            headers: {
                Authorization: "Bearer " + token
            }
        })

    if (response.status !== 200) {
        throw new Error("Something went wrong!");
    }

    return response.data;

}