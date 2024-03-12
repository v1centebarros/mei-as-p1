import { useState } from "react";
import { useAuthContext } from "../contexts/Auth.jsx";
import { Link } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { getMe } from "../api/patient.js";
import { PatientDisplay } from "../components/patientDisplay.jsx";
import { PatientEdit } from "../components/patientEdit.jsx";


export const Home = () => {

    const { token } = useAuthContext();
    const [isEditing, setIsEditing] = useState(false)

    const { data: patientData, isSuccess, isError, isLoading } = useQuery(
        {
            queryKey: ["me"],
            queryFn: async () => await getMe(token),
            refetchOnWindowFocus: false
        })

    return (
        <div>
            <h1 className="text-2xl">Home</h1>
            <Link to={"/patients"}>Patien
                {isError && <p>Error</p>}ts</Link>
            {isLoading && <p>Loading</p>}
            {isSuccess && <>
                {!isEditing ?
                    <PatientDisplay patient={patientData} />
                    :
                    <PatientEdit patient={patientData} />
                }

                <button className="btn btn-primary btn-block mt-2"
                    onClick={() => setIsEditing(!isEditing)}
                >{!isEditing ? "Edit Info" : "Cancel"} </button>

            </>
            }

        </div>
    )
}