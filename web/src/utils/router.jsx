import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { Base } from "../pages/Base";
import { Home } from "../pages/Home";
import { Login } from "../pages/Login";
import { Register } from "../pages/Register";
import { PrivateRoute } from "./PrivateRoute";
import { Patients } from "../pages/Patients";



export default function Router() {
  
    const router = createBrowserRouter([
        {
            path: "/", element: <Base />, children: [
                { index: true, element: <PrivateRoute component={Home} roles={["patient","helpdesk"]}/> },
                { path:"/patients", element: <PrivateRoute component={Patients} roles={["patient"]}/> },
            ]
        },
        { path: "/login", element: <Login /> },
        { path: "/register", element: <Register /> }
    ]);

    return <RouterProvider router={router} />;
}