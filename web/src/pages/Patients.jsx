import { useQuery } from "@tanstack/react-query"
import { useAuthContext } from "../contexts/Auth"
import { getPatients } from "../api/patient";
import { Link } from "react-router-dom";

export const Patients = () => {

    const { token } = useAuthContext();

    const { data: patientsData, isSuccess, isError, isLoading } = useQuery({
        queryKey: ["patients"],
        queryFn: async () => await getPatients(token),
        refetchOnWindowFocus: false
    })

    return <div>
        <div className="flex flex-row items-center justify-between">
            <h1 className="text-4xl font-bold">Home</h1>
            {<Link to={"/"}
                className="btn btn-primary"
            >Home</Link>}
        </div>

        {isError && <p>Error</p>}
        {isLoading && <p>Loading</p>}
        {isSuccess && <div className="overflow-x-auto">
            <table className="table">
                <thead>
                    <tr>
                        <th>Full Name</th>
                        <th>Phone Number</th>
                        <th>Diagnosis Details</th>
                        <th>Medical Number</th>
                        <th>Treatment Plan</th>
                        <th>Edit</th>
                    </tr>
                </thead>
                <tbody>
                    {patientsData.map(patient => {
                        return <tr key={patient.id}>
                            <td>{patient.fullName}</td>
                            <td>{patient.phoneNumber}</td>
                            <td>{patient.diagnosisDetails}</td>
                            <td>{patient.medicalRecordNumber}</td>
                            <td>{patient.treatmentPlan}</td>
                            <td><Link to={"/patients/edit/" + patient.id}
                                    className="btn btn-primary"
                            >Edit Patient</Link></td>
                        </tr>
                    })
                    }
                </tbody>
            </table>
        </div>}
    </div>
}