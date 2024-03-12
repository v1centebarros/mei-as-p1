import { useParams } from "react-router-dom"
import { useQuery } from "@tanstack/react-query"
import { getPatient } from "../api/patient"
import { useAuthContext } from "../contexts/Auth"
import { useState } from "react"
import { FormWithoutToken } from "../components/formWithoutToken"
import { FormWithToken } from "../components/formWithToken"

export const PatientHelpdeskEdit = () => {

    const { id } = useParams()
    const { token } = useAuthContext()

    const { data: patientData, isSuccess, isError, isLoading } = useQuery({
        queryKey: ["patient", id],
        queryFn: async () => await getPatient(token, id),
        refetchOnWindowFocus: false
    })

    const [accessToken, setAccessToken] = useState(false)

    return (
        <div className="flex flex-col gap-2">
            <div className="flex flex-row items-center justify-between">
                <h1 className="text-4xl font-bold">Home</h1>
                <button className="btn btn-primary" onClick={() => document.getElementById('accessTokenModal').showModal()}>Add Access Token</button>

            </div>

            {isError && <p>Error</p>}
            {isLoading && <p>Loading</p>}
            {isSuccess && <>
                {!accessToken ? <FormWithoutToken
                    userId={id}
                    initialData={{
                        fullName: patientData.fullName,
                        email: patientData.email,
                        medicalRecordNumber: patientData.medicalRecordNumber
                    }} /> :
                    <FormWithToken initialData={
                        {
                            fullName: patientData.fullName,
                            email: patientData.email,
                            phoneNumber: patientData.phoneNumber,
                            diagnosisDetails: patientData.diagnosisDetails,
                            medicalRecordNumber: patientData.medicalRecordNumber,
                            treatmentPlan: patientData.treatmentPlan
                        }}

                        userId={id}
                        accessToken={accessToken}
                    />
                }
            </>}

            <dialog id="accessTokenModal" className="modal">
                <div className="modal-box">
                    <form method="dialog">
                        <button className="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
                    </form>
                    <h3 className="font-bold text-lg">Add Access Token</h3>
                    <input type="text" className="input" placeholder="Access Token" onChange={(e) => setAccessToken(e.target.value)} />
                    <p className="py-4">Press ESC key or click on ✕ button to close</p>
                </div>
            </dialog>

        </div>
    )
}