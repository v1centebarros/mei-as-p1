import { useState } from "react";
import { useAuthContext } from "../contexts/Auth.jsx";
import { Link } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { getMe } from "../api/patient.js";
import { PatientDisplay } from "../components/patientDisplay.jsx";
import { PatientEdit } from "../components/patientEdit.jsx";


export const Home = () => {

    const { token, role } = useAuthContext();
    const [isEditing, setIsEditing] = useState(false)

    const { data: patientData, isSuccess, isError, isLoading } = useQuery(
        {
            queryKey: ["me"],
            queryFn: async () => await getMe(token),
            refetchOnWindowFocus: false
        })

    return (
        <div className="flex flex-col gap-2">
            <div className="flex flex-row items-center justify-between">
                <h1 className="text-4xl font-bold">Home</h1>
                {role === "helpdesk" && <Link to={"/patients"}
                    className="btn btn-primary"
                >Patients</Link>}
            </div>

            {isError && <p>Error</p>}
            {isLoading && <p>Loading</p>}
            {isSuccess && <>
                {!isEditing ?
                    <PatientDisplay patient={patientData} />
                    :
                    <PatientEdit patient={patientData} setIsEditing={setIsEditing} />
                }

                <button className="btn btn-primary btn-block mt-2"
                    onClick={() => setIsEditing(!isEditing)}
                >{!isEditing ? "Edit Info" : "Cancel"} </button>

            </>
            }

        </div>
    )
}