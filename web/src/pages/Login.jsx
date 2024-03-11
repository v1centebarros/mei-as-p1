import * as Yup from "yup";
import {ErrorMessage, Field, Form, Formik} from "formik";
import {useAuthContext} from "../contexts/Auth.jsx";
import {useNavigate} from "react-router-dom";
import { useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import { doLogin } from "../api/auth";


export const Login = () => {

    const navigate = useNavigate();
    const loginSchema = Yup.object().shape({
        email: Yup.string().email().required(),
        password: Yup.string().required()
    });
    const {token,setToken} = useAuthContext();


    const loginMutation = useMutation({
        mutationFn: (userData) => doLogin(userData),
        onSuccess: (data) => {
            setToken(()=>data.token)
            localStorage.setItem('token', data.token)
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

    const handleSubmit = (values) => {loginMutation.mutate(values)};

    return (
        <div>
            <h1 className="text-2xl">Login</h1>

            <Formik initialValues={{email: '', password: ''}} validationSchema={loginSchema}
                    onSubmit={(values) => handleSubmit(values)}>
                <Form>
                    <label className="form-control w-full max-w-xs">
                        <div>
                            <div className="label">
                                <span className="label-text">Email</span>
                            </div>
                            <ErrorMessage name={"email"}/>
                        </div>
                        <Field type="text" name="email" placeholder="email"
                               className="input input-bordered w-full max-w-xs"/>
                    </label>
                    <label className="form-control w-full max-w-xs">
                        <div>
                            <div className="label">
                                <span className="label-text">Password</span>
                            </div>
                            <ErrorMessage name={"password"}/>
                        </div>
                        <Field type="password" name="password" placeholder="password"
                               className="input input-bordered w-full max-w-xs"/>
                    </label>
                    <button type="submit" className="btn btn-primary">Login</button>
                </Form>
            </Formik>
        </div>
    )
}