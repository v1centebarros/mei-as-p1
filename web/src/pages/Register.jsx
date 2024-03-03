import * as Yup from "yup";
import { Formik, Field, ErrorMessage, Form } from "formik";
import { register } from "../../api/auth";
import { useMutation } from "@tanstack/react-query";

export const Register = () => {

    const registerSchema = Yup.object().shape({
        name: Yup.string().required('Name is required'),
        email: Yup.string().email('Invalid email').required('Email is required'),
        password: Yup.string().required('Password is required'),
        confirmPassword: Yup.string().oneOf([Yup.ref('password'), null], 'Passwords must match')
    })

    const registerMutation = useMutation({
        mutationFn:(userData) => register(userData),
        onSuccess: (data) => {
            console.log('data', data)
        },
        onError: (error) => {
            console.log('error', error)
        }
    })

    const handleSubmit = async (values) => registerMutation.mutate(values)


    return <div>
        <h1 className="text-2xl">Register</h1>
        <Formik initialValues={{ name: '', email: '', password: '', confirmPassword: '' }} validationSchema={registerSchema} onSubmit={(values) => handleSubmit(values)}>
            <Form>
                <label className="form-control w-full max-w-xs">
                    <div>
                        <div className="label">
                            <span className="label-text">Name</span>
                        </div>
                        <ErrorMessage name={"name"}/>
                    </div>
                    <Field type="text" name="name" placeholder="Full Name" className="input input-bordered w-full max-w-xs" />
                </label>

                <label className="form-control w-full max-w-xs">
                    <div>
                        <div className="label">
                            <span className="label-text">Email</span>
                        </div>
                        <ErrorMessage name={"email"} />
                    </div>
                    <Field type="text" name="email" placeholder="email@teste.com" className="input input-bordered w-full max-w-xs" />
                </label>

                <label className="form-control w-full max-w-xs">
                    <div>
                        <div className="label">
                            <span className="label-text">Password</span>
                        </div>
                        <ErrorMessage name={"password"} />
                    </div>
                    <Field type="password" name="password" placeholder="Password" className="input input-bordered w-full max-w-xs" />

                </label>

                <label className="form-control w-full max-w-xs">
                    <div>
                        <div className="label">
                            <span className="label-text">Confirm Password</span>
                        </div>
                        <ErrorMessage name={"confirmPassword"} />
                    </div>
                    <Field type="password" name="confirmPassword" placeholder="Confirm Password" className="input input-bordered w-full max-w-xs" />

                </label>

                <button type="submit" className="btn btn-primary">Register</button>
            </Form>
        </Formik>
    </div>
}
