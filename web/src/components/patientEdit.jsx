import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Formik, Form, ErrorMessage, Field } from "formik";
import { useAuthContext } from "../contexts/Auth";
import { editMe } from "../api/patient";

export const PatientEdit = ({ patient, setIsEditing}) => {

    const { token } = useAuthContext()
    const queryClient = useQueryClient()

    const editPatientMutation = useMutation({
        mutationFn: (patientData) => editMe(patientData, token),
        onSuccess: async (data) => {
            await queryClient.invalidateQueries({queryKey:['me']})
            setIsEditing(false)
        },
        onError: (error) => {
            console.log('error', error)
        }
    })


    const handleSubmit = (values) => { editPatientMutation.mutate(values) };

    return <div>
        <h1 className="text-2xl">Editing Data</h1>

        <Formik initialValues={patient}
            onSubmit={(values) => handleSubmit(values)}>

            <Form>

                <label className="form-control w-full ">
                    <div>
                        <div className="label">
                            <span className="label-text">Fullname</span>
                        </div>
                        <ErrorMessage name={"fullName"} />
                    </div>
                    <Field type="text" name="fullName" placeholder="fullName"
                        className="input input-bordered w-full " />
                </label>
                <label className="form-control w-full ">
                    <div>
                        <div className="label">
                            <span className="label-text">Email</span>
                        </div>
                        <ErrorMessage name={"email"} />
                    </div>
                    <Field type="text" name="email" placeholder="email"
                        className="input input-bordered w-full " />
                </label>


                <label className="form-control w-full ">
                    <div>
                        <div className="label">
                            <span className="label-text">Phone Number</span>
                        </div>
                        <ErrorMessage name={"phoneNumber"} />
                    </div>
                    <Field type="text" name="phoneNumber" placeholder="phoneNumber"
                        className="input input-bordered w-full " />
                </label>

                <label className="form-control w-full ">
                    <div>
                        <div className="label">
                            <span className="label-text">Diagnosis Details</span>
                        </div>
                        <ErrorMessage name={"diagnosisDetails"} />
                    </div>
                    <Field type="text" name="diagnosisDetails" placeholder="diagnosisDetails"
                        className="input input-bordered w-full " />
                </label>

                <label className="form-control w-full ">
                    <div>
                        <div className="label">
                            <span className="label-text">Medical Record Number</span>
                        </div>
                        <ErrorMessage name={"medicalRecordNumber"} />
                    </div>
                    <Field type="text" name="medicalRecordNumber" placeholder="medicalRecordNumber"
                        className="input input-bordered w-full " />
                </label>


                <label className="form-control w-full ">
                    <div>
                        <div className="label">
                            <span className="label-text">Treatment Plan</span>
                        </div>
                        <ErrorMessage name={"treatmentPlan"} />
                    </div>
                    <Field type="text" name="treatmentPlan" placeholder="treatmentPlan"
                        className="input input-bordered w-full " />
                </label>

                <button type="submit" className="btn btn-success btn-block mt-2">Submit</button>
            </Form>
        </Formik>

    </div>
}