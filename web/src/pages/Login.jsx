import * as Yup from "yup";
import { ErrorMessage, Field, Form, Formik } from "formik";
import { useAuthContext } from "../contexts/Auth.jsx";
import { useNavigate, Link } from "react-router-dom";
import { useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import { doLogin } from "../api/auth";
import { jwtDecode } from 'jwt-decode';


export const Login = () => {

    const navigate = useNavigate();
    const loginSchema = Yup.object().shape({
        email: Yup.string().email().required(),
        password: Yup.string().required()
    });
    const { token, setToken, setRole } = useAuthContext();


    const loginMutation = useMutation({
        mutationFn: (userData) => doLogin(userData),
        onSuccess: (data) => {
            setToken(() => data.token)
            localStorage.setItem('token', data.token)
            setRole(() => jwtDecode(data.token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"])
            localStorage.setItem('role', jwtDecode(data.token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"])
        },
        onError: (error) => {
            console.log('error', error)
        }
    });

    useEffect(() => {
        if (token) {
            navigate("/")
        }
    }, [token])

    const handleSubmit = (values) => { loginMutation.mutate(values) };

    return (
        <div className="mx-auto bg-base-200 min-h-screen h-full flex items-center">
            <div className="container mx-auto card bg-white">
                <h1 className="text-5xl font-bold mx-auto">Login</h1>

                <Formik initialValues={{ email: '', password: '' }} validationSchema={loginSchema}
                    onSubmit={(values) => handleSubmit(values)}>
                    <Form className="flex flex-col gap-y-2 m-2">
                        <label className="form-control w-full">
                            <div>
                                <div className="label">
                                    <span className="label-text">Email</span>
                                </div>
                                <ErrorMessage name={"email"} />
                            </div>
                            <Field type="text" name="email" placeholder="email"
                                className="input input-bordered w-full" />
                        </label>
                        <label className="form-control w-full">
                            <div>
                                <div className="label">
                                    <span className="label-text">Password</span>
                                </div>
                                <ErrorMessage name={"password"} />
                            </div>
                            <Field type="password" name="password" placeholder="password"
                                className="input input-bordered w-full" />
                        </label>
                        <button type="submit" className="btn btn-primary btn-block">Login</button>
                        <Link to={"/register"} type="submit" className="btn btn-primary btn-block">I am New!</Link>
                    </Form>
                </Formik>
            </div>
        </div>
    )
}