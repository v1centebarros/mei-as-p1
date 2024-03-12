import { Outlet } from "react-router-dom"
import { useAuthContext } from "../contexts/Auth"

export const Base = () => {
    const { logout } = useAuthContext();
    return (
        <div className="mx-auto bg-base-200 min-h-screen h-full">
            <div className="navbar bg-base-100 rounded-xl">
                <div className="flex-1">
                    <a className="btn btn-ghost text-xl">Patient Inc.</a>
                </div>
                <div className="flex-none">
                    <ul className="menu menu-horizontal px-1">
                        <li>            <button className="btn btn-primary" onClick={() => logout()}>Logout</button></li>
                    </ul>
                </div>
            </div>

            <div className="container mx-auto pt-2">
                <Outlet />
            </div>

        </div>
    )
}