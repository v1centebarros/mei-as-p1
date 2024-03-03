import {createBrowserRouter, RouterProvider} from "react-router-dom";
import {Home} from "@/pages/home";

export default function Router() {

    const router = createBrowserRouter([{
        path: "/",
        element: <Home />
    }])
    return <RouterProvider router={router} />
}
