import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { Base } from "../pages/Base";
import { Home } from "../pages/Home";
import { Login } from "../pages/Login";
import { Register } from "../pages/Register";



export default function Router() {
  
    const router = createBrowserRouter([
        {
            path: "/", element: <Base />, children: [
                { index: true, element: <Home /> },
            ]
        },
        { path: "/login", element: <Login /> },
        { path: "/register", element: <Register /> }
    ]);

    return <RouterProvider router={router} />;
}