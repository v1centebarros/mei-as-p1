import * as Yup from "yup";
import { Formik, Field, ErrorMessage, Form } from "formik";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@tanstack/react-query";
import { doRegister } from "../api/auth";
import { Link } from "react-router-dom";


export const Register = () => {

    const navigate = useNavigate()
    const registerSchema = Yup.object().shape({
        fullName: Yup.string().required('Full Name is required'),
        email: Yup.string().email('Invalid email').required('Email is required'),
        password: Yup.string().required('Password is required'),
        confirmPassword: Yup.string().oneOf([Yup.ref('password'), null], 'Passwords must match'),
        treatmentPlan: Yup.string().required("Please fill the Treatment Plan"),
        phoneNumber: Yup.string().required("Please fill the Phone Number"),
        diagnosisDetails: Yup.string().required("Please fill the Diagnosis Details"),
        accessCode: Yup.string().required("Please fill the Access Code")

    })


    const registerMutation = useMutation({
        mutationFn: (userData) => doRegister(userData),
        onSuccess: (data) => {
            navigate('/login')
        },
        onError: (error) => {
            console.log('error', error)
        }
    })

    const handleSubmit = (values) => registerMutation.mutate(values)


    return <div className="mx-auto bg-base-200 min-h-screen h-full flex items-center">
        <div className="container mx-auto card bg-white">
            <h1 className="text-5xl font-bold mx-auto">Register</h1>
            <Formik initialValues={{ fullName: '', email: '', password: '', confirmPassword: '', treatmentPlan: '', phoneNumber: '', diagnosisDetails: '', accessCode: '' }} validationSchema={registerSchema} onSubmit={(values) => handleSubmit(values)}>
                <Form className="flex flex-col gap-y-2 m-2">
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
                                <span className="label-text">Password</span>
                            </div>
                            <ErrorMessage name={"password"} />
                        </div>
                        <Field type="password" name="password" placeholder="Password" className="input input-bordered w-full" />

                    </label>

                    <label className="form-control w-full">
                        <div>
                            <div className="label">
                                <span className="label-text">Confirm Password</span>
                            </div>
                            <ErrorMessage name={"confirmPassword"} />
                        </div>
                        <Field type="password" name="confirmPassword" placeholder="Confirm Password" className="input input-bordered w-full" />

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

                    <label className="form-control w-full">
                        <div>
                            <div className="label">
                                <span className="label-text">Phone Number</span>
                            </div>
                            <ErrorMessage name={"phoneNumber"} />
                        </div>
                        <Field type="text" name="phoneNumber" placeholder="Phone Number" className="input input-bordered w-full" />
                    </label>

                    <label className="form-control w-full">
                        <div>
                            <div className="label">
                                <span className="label-text">Diagnosis Details</span>
                            </div>
                            <ErrorMessage name={"diagnosisDetails"} />
                        </div>
                        <Field type="text" name="diagnosisDetails" placeholder="Diagnosis Details" className="input input-bordered w-full" />
                    </label>


                    <label className="form-control w-full">
                        <div>
                            <div className="label">
                                <span className="label-text">Access Code</span>
                            </div>
                            <ErrorMessage name={"accessCode"} />
                        </div>
                        <Field type="text" name="accessCode" placeholder="Access Code" className="input input-bordered w-full" />
                    </label>

                    <button type="submit" className="btn btn-primary">Register</button>
                    <Link to={"/login"} type="submit" className="btn btn-primary btn-block">Already Registered</Link>

                </Form>
            </Formik>
        </div>
    </div>
}
