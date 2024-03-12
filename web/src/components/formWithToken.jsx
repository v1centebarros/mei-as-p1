import { useMutation } from "@tanstack/react-query"
import { useAuthContext } from "../contexts/Auth";
import { Field, ErrorMessage, Form, Formik } from "formik";
import * as Yup from "yup";
import { updateWithAccessToken } from "../api/patient";
import { Link, useNavigate } from "react-router-dom";


export const FormWithToken = ({ initialData, userId, accessToken }) => {

    const navigate = useNavigate()
    const validationSchema = Yup.object().shape({
        fullName: Yup.string().required('Full Name is required'),
        email: Yup.string().email('Invalid email').required('Email is required'),
        phoneNumber: Yup.string().required('Phone is required'),
        diagnosisDetails: Yup.string().required('Diagnosis Details is required'),
        medicalRecordNumber: Yup.string().required('Full Name is required'),
        treatmentPlan: Yup.string().required('Treatment Plan is required')
    })

    const { token } = useAuthContext()
    const updateWithAccessTokenMutation = useMutation({
        mutationFn: (userData) => updateWithAccessToken(token, userData, userId, accessToken),
        onSuccess: (data) => {
            navigate("/patients")
        },
        onError: (error) => {
            console.log("error");
        }
    })

    const handleSubmit = (values) => updateWithAccessTokenMutation.mutate(values)
    return <Formik initialValues={initialData} validationSchema={validationSchema} onSubmit={(values) => handleSubmit(values)}>
        <Form>
            <label className="form-control w-full">
                <div>
                    <div className="label">
                        <span className="label-text">Name</span>
                    </div>
                    <ErrorMessage name={"fullName"} />
                </div>
                <Field type="text" name="fullName" placeholder="Full Name" className="input input-bordered w-full" />
            </label>

            <label className="form-control w-full">
                <div>
                    <div className="label">
                        <span className="label-text">Email</span>
                    </div>
                    <ErrorMessage name={"email"} />
                </div>
                <Field type="text" name="email" placeholder="email@teste.com" className="input input-bordered w-full" />
            </label>

            <label className="form-control w-full">
                <div>
                    <div className="label">
                        <span className="label-text">Phone</span>
                    </div>
                    <ErrorMessage name={"phoneNumber"} />
                </div>
                <Field type="text" name="phoneNumber" placeholder="9111111" className="input input-bordered w-full" />
            </label>

            <label className="form-control w-full">
                <div>
                    <div className="label">
                        <span className="label-text">Diagnosis Details</span>
                    </div>
                    <ErrorMessage name={"phone"} />
                </div>
                <Field type="text" name="diagnosisDetails" placeholder="salkddslk" className="input input-bordered w-full" />
            </label>

            <label className="form-control w-full">
                <div>
                    <div className="label">
                        <span className="label-text">Medical Record Number</span>
                    </div>
                    <ErrorMessage name={"medicalRecordNumber"} />
                </div>
                <Field type="text" name="medicalRecordNumber" placeholder="Medical Record Number" className="input input-bordered w-full" />
            </label>

            <label className="form-control w-full">
                <div>
                    <div className="label">
                        <span className="label-text">Treatment Plan</span>
                    </div>
                    <ErrorMessage name={"treatmentPlan"} />
                </div>
                <Field type="text" name="treatmentPlan" placeholder="Treatment Plan" className="input input-bordered w-full" />
            </label>

            <button type="submit" className="btn btn-success btn-block mt-2">Update Patient</button>
            <Link to="/patients" className="btn btn-primary btn-block mt-2">Cancel</Link>

        </Form>
    </Formik>


}