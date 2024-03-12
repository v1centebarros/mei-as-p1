import { useMutation } from "@tanstack/react-query"
import { useAuthContext } from "../contexts/Auth";
import { Field, ErrorMessage, Form, Formik } from "formik";
import * as Yup from "yup";
import { updateWithoutAccessToken } from "../api/patient";
import { Link, useNavigate } from "react-router-dom";


export const FormWithoutToken = ({ initialData, userId }) => {

    const navigate = useNavigate()
    const validationSchema = Yup.object().shape({
        fullName: Yup.string().required('Full Name is required'),
        email: Yup.string().email('Invalid email').required('Email is required'),
        medicalRecordNumber:Yup.string().required('Full Name is required'),

    })

    const { token } = useAuthContext()
    const updateWithoutAccessTokenMutation = useMutation({
        mutationFn: (userData) => updateWithoutAccessToken(token, userData, userId),
        onSuccess: (data) => {
            navigate("/patients")
        },
        onError: (error) => {
            console.log("error");
        }
    })

    const handleSubmit = (values) => updateWithoutAccessTokenMutation.mutate(values)
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
                            <span className="label-text">Medical Record Number</span>
                        </div>
                        <ErrorMessage name={"medicalRecordNumber"}/>
                    </div>
                    <Field type="text" name="medicalRecordNumber" placeholder="Medical Record Number" className="input input-bordered w-full" />
                </label>
            <button type="submit" className="btn btn-success btn-block mt-2">Update Patient</button>
            <Link to="/patients" className="btn btn-primary btn-block mt-2">Cancel</Link>

        </Form>
    </Formik>


}