export const PatientDisplay = ({ patient }) => {
    return <div className="card w-full bg-base-100 shadow-xl">
        <div className="card-body">
            <h2 className="card-title">{patient.fullName}</h2>
            <p><span className="font-bold">Email:</span> {patient.email}</p>
            <p><span className="font-bold">Phone Number:</span> {patient.phoneNumber}</p>
            <p><span className="font-bold">Diagnosis Details:</span> {patient.diagnosisDetails}</p>
            <p><span className="font-bold">Medical Record Number:</span> {patient.medicalRecordNumber}</p>
            <p><span className="font-bold">Treatment Plan:</span> {patient.treatmentPlan}</p>
        </div>

    </div>
}